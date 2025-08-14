using System;

namespace IT.TnDigit.ORM.ClientController
{
    public static class HelperConverter
    {
        public static bool IsDate(object data)
        {
            DateTime value;
            return DateTime.TryParse(data.ToString(), out value);
        }

        public static bool IsNumber(object data)
        {
            Decimal value;
            return Decimal.TryParse(data.ToString(), out value);
        }
    }
}
