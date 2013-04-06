namespace Quad.Berm.Web.Mvc.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public static class ControllerHelper
    {
        public static SelectList ToSelectList<TModel>(
            this IEnumerable<TModel> enumerable,
            long selectedValue,
            Func<long, string, TModel> produce,
            Func<long, TModel, bool> predicate,
            string dataValueField = "Id",
            string dataTextField = "Name",
            bool allowMissed = true,
            bool allowEmpty = true,
            string undefinedText = "-Undefined-",
            string emptyText = "")
        {
            var list = new List<TModel>(enumerable);
            var isMissed = selectedValue != 0 && !list.Any(m => predicate(selectedValue, m));
            if (isMissed)
            {
                if (allowMissed)
                {
                    list.Add(produce(selectedValue, undefinedText));
                }
                else
                {
                    allowEmpty = true;
                }
            }

            if (allowEmpty)
            {
                list.Insert(0, produce(0, emptyText));
            }

            var result = new SelectList(list, dataValueField, dataTextField, selectedValue);
            return result;
        }
    }
}