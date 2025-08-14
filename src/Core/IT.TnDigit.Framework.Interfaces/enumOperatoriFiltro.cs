namespace IT.TnDigit.ORM.Interfaces
{
    public static class TranslateOperatoriFiltro
    {
        public static string Get(OperatoriFiltro op)
        {
            switch (op)
            {
                case OperatoriFiltro.Uguale:
                    return "{0} = {1}";
                case OperatoriFiltro.Diverso:
                    return "{0} <> {1}";
                case OperatoriFiltro.Maggiore:
                    return "{0} > {1}";
                case OperatoriFiltro.Minore:
                    return "{0} < {1}";
                case OperatoriFiltro.MaggioreUguale:
                    return "{0} >= {1}";
                case OperatoriFiltro.MinoreUguale:
                    return "{0} <= {1}";
                case OperatoriFiltro.Simile:
                    return "UPPER({0}) = UPPER({1})";
                case OperatoriFiltro.Contiene:
                case OperatoriFiltro.IniziaCon:
                case OperatoriFiltro.FinisceCon:
                    return "UPPER({0}) LIKE UPPER({1})";
                case OperatoriFiltro.In:
                    return "{0} IN({1})";
                case OperatoriFiltro.NotIn:
                    return "{0} NOT IN({1})";
                case OperatoriFiltro.Contains:
                    return "CONTAINS({0}, '{1}', 1)>0 ";
                default:
                    return "{0} = {1}";
            }
        }

        public static string GetTruncDate(OperatoriFiltro op)
        {
            string operation = "";
            switch (op)
            {
                case OperatoriFiltro.Uguale:
                    operation = "=";
                    break;
                case OperatoriFiltro.Diverso:
                    operation = "<>";
                    break;
                case OperatoriFiltro.Maggiore:
                    operation = ">";
                    break;
                case OperatoriFiltro.Minore:
                    operation = "<";
                    break;
                case OperatoriFiltro.MaggioreUguale:
                    operation = ">=";
                    break;
                case OperatoriFiltro.MinoreUguale:
                    operation = "<=";
                    break;
                default:
                    operation = "=";
                    break;
            }

            return "TRUNC({0},'DDD') " + operation + " TRUNC({1},'DDD')";
        }

        public static string Get(OperatoriFiltroAvanzati op)
        {
            switch (op)
            {
                case OperatoriFiltroAvanzati.FuzzyContain:
                    return "CONTAINS({0}, '{1}', 1)>0 ";
                default:
                    return "CONTAINS({0}, '{1}', 1)>0";
            }
        }

        public static string Get(OperatoriFiltroMultipli op)
        {
            switch (op)
            {
                case OperatoriFiltroMultipli.Between:
                    return "({0} BETWEEN {1} AND {2}) ";
                default:
                    return "({0} BETWEEN {1} AND {2}) ";
            }
        }

        public static string GetTruncDate(OperatoriFiltroMultipli op)
        {
            switch (op)
            {
                case OperatoriFiltroMultipli.Between:
                    return "(TRUNC({0},'DDD') BETWEEN TRUNC({1},'DDD') AND TRUNC({2},'DDD')) ";
                default:
                    return "(TRUNC({0},'DDD') BETWEEN TRUNC({1},'DDD') AND TRUNC({2},'DDD')) ";
            }
        }

        public static string Get(OperatoriContainsMultipli op)
        {
            switch (op)
            {
                case OperatoriContainsMultipli.ContainsAND:
                case OperatoriContainsMultipli.ContainsOR:
                default:
                    return "(CONTAINS ({0}, {1}) >0)";
            }
        }
    }

    public enum OperatoriFiltro
    {
        Uguale, Diverso, Maggiore, Minore, MaggioreUguale,
        MinoreUguale, Contiene, IniziaCon, FinisceCon, Simile, In, NotIn, Contains
    }

    public enum OperatoriFiltroAvanzati { FuzzyContain }

    public enum OperatoriFiltroMultipli { Between }

    public enum OperatoriContainsMultipli { ContainsOR, ContainsAND }
}
