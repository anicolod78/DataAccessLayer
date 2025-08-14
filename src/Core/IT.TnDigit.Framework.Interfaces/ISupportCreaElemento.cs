namespace IT.TnDigit.ORM.Interfaces
{
    public abstract class SupportCreaElemento
    {
        private DBOperation dbOp;

        public DBOperation DBop
        {
            get { return dbOp; }
            set { dbOp = value; }
        }


        public virtual object CreaElemento(System.Data.IDataRecord dati)
        {
            return null;
        }
    }

    public enum DBOperation
    {
        NoOp = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}
