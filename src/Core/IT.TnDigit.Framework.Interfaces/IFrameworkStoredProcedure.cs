using System;
using System.Data;
namespace IT.TnDigit.ORM.Interfaces
{
    public interface IFrameworkStoredProcedure
    {
        bool ReturnTable { get; set; }
        void AddInputParameter(FrameworkStoredProcedureParameter param);
        void AddOutputParameter(FrameworkStoredProcedureParameter param);
        void AddInOutParameter(FrameworkStoredProcedureParameter param);
        FrameworkStoredProcedureParameter GetInputParameter(string nome);
        FrameworkStoredProcedureParameter GetOutputParameter(string nome);
        System.Collections.Generic.Dictionary<string, FrameworkStoredProcedureParameter> Input { get; }
        string Nome { get; set; }
        System.Collections.Generic.Dictionary<string, FrameworkStoredProcedureParameter> Output { get; }
        IFrameworkCollection TableResult { get; set; }
    }


    public class FrameworkStoredProcedureParameter
    {
        private string parameterName = "";
        private ParameterDirection direzione = ParameterDirection.Input;
        private DbType tipo = DbType.String;
        private object valore = null;
        private Int32 size = 255;
        public bool useTime { get; set; }

        public Int32 Lunghezza
        {
            get { return size; }
            set { size = value; }
        }

        public FrameworkStoredProcedureParameter(string nome, ParameterDirection direzione, DbType tipo, object valore)
        {
            this.parameterName = nome;
            this.direzione = direzione;
            this.tipo = tipo;
            this.valore = valore;
            this.size = 255;
        }

        public FrameworkStoredProcedureParameter(string nome, ParameterDirection direzione, DbType tipo, object valore, Int32 lunghezza)
        {
            this.parameterName = nome;
            this.direzione = direzione;
            this.tipo = tipo;
            this.valore = valore;
            this.size = lunghezza;
        }

        public FrameworkStoredProcedureParameter(string nome, ParameterDirection direzione, DbType tipo, object valore, bool useTime)
        {
            this.parameterName = nome;
            this.direzione = direzione;
            this.tipo = tipo;
            this.valore = valore;
            this.size = 255;
            this.useTime = useTime;
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public ParameterDirection Direzione
        {
            get { return direzione; }
        }

        public DbType Tipo
        {
            get { return tipo; }
        }

        public object Valore
        {
            get
            {
                return this.valore;
            }
            set
            {
                this.valore = value;
            }
        }
    }
}
