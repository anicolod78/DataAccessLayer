using IT.TnDigit.ORM.Interfaces;
using System.Collections.Generic;


namespace IT.TnDigit.ORM.DataTypes
{

    public abstract class FrameworkStoredProcedure : IFrameworkStoredProcedure
    {
        private string nome;
        private bool returnTable;
        private IFrameworkCollection tableResult;
        protected Dictionary<string, FrameworkStoredProcedureParameter> input = new Dictionary<string, FrameworkStoredProcedureParameter>();
        protected Dictionary<string, FrameworkStoredProcedureParameter> output = new Dictionary<string, FrameworkStoredProcedureParameter>();
        protected Dictionary<string, FrameworkStoredProcedureParameter> inout = new Dictionary<string, FrameworkStoredProcedureParameter>();

        public virtual string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public Dictionary<string, FrameworkStoredProcedureParameter> Input
        {
            get
            {
                return this.input;
            }
        }

        public Dictionary<string, FrameworkStoredProcedureParameter> Output
        {
            get
            {
                return this.output;
            }
        }


        public Dictionary<string, FrameworkStoredProcedureParameter> InOut
        {
            get { return this.inout; }
        }

        public void AddInputParameter(FrameworkStoredProcedureParameter param)
        {
            input.Add(param.ParameterName, param);
        }

        public void AddOutputParameter(FrameworkStoredProcedureParameter param)
        {
            output.Add(param.ParameterName, param);
        }

        public void AddInOutParameter(FrameworkStoredProcedureParameter param)
        {
            inout.Add(param.ParameterName, param);
        }


        public FrameworkStoredProcedureParameter GetInputParameter(string nome)
        {
            if (input.ContainsKey(nome) == true)
                return input[nome];

            return null;
        }

        public FrameworkStoredProcedureParameter GetInOutParameter(string nome)
        {
            if (inout.ContainsKey(nome) == true)
                return inout[nome];

            return null;
        }

        public FrameworkStoredProcedureParameter GetOutputParameter(string nome)
        {
            if (output.ContainsKey(nome) == true)
                return output[nome];

            return null;
        }


        public bool ReturnTable
        {
            get
            {
                return returnTable;
            }
            set
            {
                returnTable = value;
            }
        }


        public IFrameworkCollection TableResult
        {
            get
            {
                return tableResult;
            }
            set
            {
                tableResult = value;
            }
        }
    }
}

