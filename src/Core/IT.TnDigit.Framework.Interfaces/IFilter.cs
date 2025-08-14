using System;
using System.Collections.Generic;

namespace IT.TnDigit.ORM.Interfaces
{
    public interface IFilter
    {
        void AppendSQL(string SQL);

        void Add(string campo, OperatoriFiltroMultipli operatore, object valore1, object valore2);

        void Add(string campo, OperatoriFiltroMultipli operatore, object valore1, object valore2, bool useTime);

        void Add(string campo, OperatoriFiltro operatore, object valore);

        void Add(string campo, OperatoriFiltro operatore, DateTime? valore, bool useTime);

        void Add(string campo, OperatoriFiltroAvanzati operatore, object valore, int score);

        void Add(string campo, OperatoriFiltro operatore, object valore, string distintivo);

        void Add(string key, string filter);

        string[] FiltriApplicati();

        void Reset();

        string ToString();

        string ToNonParametricString();

        List<QueryParameter> Parameters { get; }

        IFilter CreateNewFilter();
    }

    public struct QueryParameter
    {
        private string parameterName;

        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        private object val;

        public object Value
        {
            get { return val; }
            set { val = value; }
        }

        public bool useTime { get; set; }
    }
}
