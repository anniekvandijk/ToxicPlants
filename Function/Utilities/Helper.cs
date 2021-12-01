namespace Function.Utilities
{
    internal static class Helper
    {
        public static string TrimToNull(this string value)
        {
            if (value == null)
                return null;
            else if (value.Trim().Length == 0)
                return null;
            else
                return value.Trim();
        }
    }
}
