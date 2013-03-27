namespace Quad.Berm.Common.Utils
{
    using System.Globalization;

    public static class StringUtils
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fmt", Justification = "By design")]
        public static string Fmt(this string format, params object[] args)
        {
            return string.IsNullOrEmpty(format) ? string.Empty : string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}