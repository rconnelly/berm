namespace Quad.Berm.Common.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    public static class UnityExtension
    {
        private static readonly Func<UnityContainer, Dictionary<Type, List<string>>> GetRegisteredKeys;

        private static readonly ResolverOverride[] EmptyResolverOverrides = new ResolverOverride[0];

        static UnityExtension()
        {
            // Get information about fileds.
            var unityType = typeof(UnityContainer);
            var registryType = Type.GetType("Microsoft.Practices.Unity.NamedTypesRegistry, " + unityType.Assembly.FullName);
            Contract.Assert(registryType != null);

            const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var registeredNamesInfo = unityType.GetField("registeredNames", Flags);
            Contract.Assert(registeredNamesInfo != null);
            var registeredKeysInfo = registryType.GetField("registeredKeys", Flags);
            Contract.Assert(registeredKeysInfo != null);

            // Create and compile expression to accessing the field
            // UnityContainer.registeredNames.registeredKeys of type Dictionary<Type, List<string>>
            var unityParam = Expression.Parameter(unityType);
            GetRegisteredKeys = Expression
                .Lambda<Func<UnityContainer, Dictionary<Type, List<string>>>>(
                    Expression.Field(
                        Expression.Field(
                            unityParam,
                            registeredNamesInfo),
                        registeredKeysInfo),
                    unityParam)
                .Compile();
        }

        /// <summary>
        ///     Try to resolve an instance of requested type <typeparamref name="T" /> without name.
        ///     If type is interface or abstract class and it isn't registered then return null.
        /// </summary>
        public static T TryResolve<T>(this IUnityContainer container) where T : class
        {
            return (T)TryResolve(container, typeof(T));
        }

        /// <summary>
        ///     Try to resolve an instance of requested <paramref name="type" /> without name.
        ///     If type is interface or abstract class and it isn't registered then return null.
        /// </summary>
        public static object TryResolve(this IUnityContainer container, Type type)
        {
            bool resolve;
            if (type.IsInterface || type.IsAbstract)
            {
                // Get the dictionary with registered types and names.
                var keys = GetRegisteredKeys((UnityContainer)container);

                // Check if interface or abstract type is registered in the container.
                resolve = IsRegistered(type, keys);

                // If type still is't registered and it's generic type then check if generic type definition is registered.
                if (!resolve && type.IsGenericType)
                {
                    var openGenericType = type.GetGenericTypeDefinition();
                    resolve = IsRegistered(openGenericType, keys);
                }
            }
            else
            {
                // If type can be created then resolve it immediately.
                resolve = true;
            }

            // If type is registered then resolve it or return null if nothing was found.
            var result = resolve ? container.Resolve(type, null, EmptyResolverOverrides) : null;
            return result;
        }

        private static bool IsRegistered(Type type, IReadOnlyDictionary<Type, List<string>> keys)
        {
            var result = false;
            List<string> names;
            if (keys.TryGetValue(type, out names))
            {
                // By default type without name is registered with null string.
                result = names.Exists(name => ReferenceEquals(name, null));
            }

            return result;
        }
    }
}