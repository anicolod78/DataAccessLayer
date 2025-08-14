namespace IT.TnDigit.ORM.Configurator.Classi
{
    public static class StringExtensions
    {
        private static string Clear(string value)
        {
            return value.Trim().Replace(" ", "").Replace("-", "_").Replace("%", "perc").Replace("/", "_");
        }

        public static string ClearInvalidCharsU(this string value)
        {
            return Clear(value).ToUpper();
        }

        public static string ClearInvalidCharsL(this string value)
        {
            return Clear(value).ToLower();
        }
    }
}
