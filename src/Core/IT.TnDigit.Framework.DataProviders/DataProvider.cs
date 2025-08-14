#region Using directives

using IT.TnDigit.ORM.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public abstract partial class DataProvider : IDataProvider
    {
        protected IDbConnection connection = null;
        protected string connectionString;
        protected string alwaysAllowDateTime = "DATAINSERIMENTO,DATAMODIFICA,DATACANCELLAZIONE,DELETEDDATE,DELETED";


        public void Initialize(string initString)
        {
            this.connectionString = initString;
        }

        public void Close()
        {
            if (this.connection != null)
            {
                this.connection.Close();
            }
        }


        public ConnectionState GetConnectionState()
        {
            if (this.connection == null)
            {
                return ConnectionState.Broken;
            }
            else
            {
                return this.connection.State;
            }
        }

        #region protected Members

        public virtual System.Data.IDbConnection CreateConnection()
        {
            return null;
        }

        protected virtual System.Data.IDbConnection GetConnection()
        {
            return this.connection;
        }

        public virtual string parameterPlaceHolder
        {
            get
            {
                return ":P";
            }
        }

        protected virtual int SizeLimitedField(System.Reflection.PropertyInfo prop)
        {
            return 0;
        }

        protected bool AllowDateTimeField(System.Reflection.PropertyInfo prop)
        {
            if (!string.IsNullOrEmpty(alwaysAllowDateTime)
                && string.Equals(alwaysAllowDateTime, "true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            Object[] attributes = prop.GetCustomAttributes(true);
            foreach (Object att in attributes)
            {
                if ((Attribute)att is AllowDateTime)
                {
                    return ((AllowDateTime)att).AllowTime;
                }
            }

            return false;
        }

        protected bool AllowDateTimeField(FrameworkStoredProcedureParameter Fparam)
        {
            return Fparam.useTime;
        }

        protected bool IsPrimaryKey(System.Reflection.PropertyInfo prop)
        {
            Object[] attributes = prop.GetCustomAttributes(true);
            foreach (Object att in attributes)
            {
                if ((Attribute)att is PrimaryKey)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsMappedField(System.Reflection.PropertyInfo prop)
        {
            Object[] attributes = prop.GetCustomAttributes(true);
            foreach (Object att in attributes)
            {
                if ((Attribute)att is MappedField)
                {
                    return true;
                }
            }
            return false;
        }


        protected bool IsNullableForeignKey(System.Reflection.PropertyInfo prop)
        {
            Object[] attributes = prop.GetCustomAttributes(true);
            foreach (Object att in attributes)
            {
                if ((Attribute)att is NullableForeignKey)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual string JollyCharacter
        {
            get
            {
                return "*";
            }
        }



        protected virtual string SelectIdentity(string SequenceName)
        {
            return "SELECT @@IDENTITY";
        }

        #endregion

        #region private Members

        protected string GetTableName(Type itemType)
        {
            try
            {
                IFrameworkObject obj = (IFrameworkObject)Type.GetType(itemType.AssemblyQualifiedName).GetConstructor(new Type[] { }).Invoke(null);
                return obj.TableName();
            }
            catch
            {
                return null;
            }
        }

        protected string GetTableName(SupportCreaElemento obj)
        {
            try
            {
                return ((IFrameworkObject)obj).TableName();
            }
            catch
            {
                return null;
            }
        }

        protected string GetFieldsList(SupportCreaElemento obj)
        {
            try
            {
                return ((IFrameworkObject)obj).FieldsList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return null;
            }
        }

        protected List<string> GetPropertiesChanged(IDataRecord obj)
        {
            try
            {
                return ((IFrameworkObject)((DataItem)obj).GetItem).PropertiesChanged;
            }
            catch
            {
                return new List<string>();
            }
        }

        protected string GetAggiunteSelect(SupportCreaElemento obj)
        {
            try
            {
                IFrameworkObjectExtension obj2 = obj as IFrameworkObjectExtension;
                if (obj2 == null)
                    return null;

                string result = obj2.AggiunteSelect();
                if (result == null || result == String.Empty)
                    return null;
                else
                    return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return null;
            }
        }

        #endregion

        public virtual string DateFilter(string data)
        {
            return string.Format("TO_DATE('{0}','dd/MM/yyyy')", data);
        }

        public virtual string DateTimeFilter(string data)
        {
            return string.Format("TO_DATE('{0}','dd/MM/yyyy HH:mm:ss')", data);
        }

        public virtual string DateFilterTruncateField(string campo)
        {
            return campo;
        }

        protected virtual void SetLongStringParamType(IDbDataParameter param)
        {
            return;
        }

        public virtual object TranslateBooleanValue(object val)
        {
            return val;
        }

        protected virtual void ActivateBindByName(IDbCommand cmd)
        {
            return;
        }

        protected static void FillQueryParameter(IFilter filtro, IDbCommand cmd)
        {
            if (cmd == null || filtro == null)
                return;

            string s = cmd.CommandText;
            System.Diagnostics.Debug.WriteLine("".PadRight(40, '-'));
            System.Diagnostics.Debug.WriteLine("+ORACLE PARAMETRIZED Command = " + s);

            foreach (QueryParameter p in filtro.Parameters)
            {
                IDbDataParameter par = cmd.CreateParameter();
                par.ParameterName = p.ParameterName;
                if (p.Value == null)
                    par.Value = DBNull.Value;
                else
                    par.Value = p.Value;
                cmd.Parameters.Add(par);

                if (par.Value == DBNull.Value)
                {
                    s = s.Replace(par.ParameterName, "NULL");
                    System.Diagnostics.Debug.WriteLine("|\tParam = " + p.ParameterName + " -> " + "NULL");
                }
                else
                {
                    s = s.Replace(par.ParameterName, par.Value.ToString());
                    System.Diagnostics.Debug.WriteLine("|\tParam = " + p.ParameterName + " -> " + p.Value.ToString());
                }
            }
            System.Diagnostics.Debug.WriteLine("+ORACLE NON PARAMETRIZED Command = " + s);
            System.Diagnostics.Debug.WriteLine("".PadRight(40, '-'));
        }

        #region GET ITEMS

        public IEnumerable GetItems(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx)
        {
            IDataReader reader = null;
            IDbCommand cmd = null;
            IDbConnection conn = null;
            if (tx == null)
                conn = this.GetConnection();
            else
                conn = this.connection;

            cmd = GetItemsCommand(conn, prototype, filtro, ordinamento, elementiPerPagina, pagina, ref risultati);
            if (cmd == null)
                return null;

            if (conn.State != ConnectionState.Open)
                conn.Open();

            if (tx == null)
            {
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            else
            {
                cmd.Transaction = tx;
                reader = cmd.ExecuteReader();
            }
            return (IEnumerable)reader;
        }

        public IEnumerable GetItems(SupportCreaElemento prototype, IFilter filtro, string ordinamento, IDbTransaction tx)
        {
            IDataReader reader = null;
            IDbCommand cmd = null;
            IDbConnection conn = null;
            if (tx == null)
            {
                conn = this.GetConnection();

                cmd = GetItemsCommand(conn, prototype, filtro.ToString(), ordinamento);
                ActivateBindByName(cmd);
                FillQueryParameter(filtro, cmd);

                if (cmd == null)
                    return null;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;

                cmd = GetItemsCommand(conn, prototype, filtro.ToString(), ordinamento);
                ActivateBindByName(cmd);
                FillQueryParameter(filtro, cmd);

                if (cmd == null)
                    return null;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                reader = cmd.ExecuteReader();

                ((IDisposable)cmd).Dispose();

            }
            return (IEnumerable)reader;
        }


        protected virtual IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, string filtro, string ordinamento)
        {
            IDbCommand cmd = conn.CreateCommand();

            string cmdText = "Select {4}{3} From {0}{1}{2}";

            if (filtro != null && filtro != "")
            {
                filtro = " Where " + filtro;
                filtro = filtro.Replace("*", JollyCharacter);
            }

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            if (this.GetAggiunteSelect(prototype) != null)
                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), filtro, ordinamento, "," + this.GetAggiunteSelect(prototype), this.GetFieldsList(prototype));
            else
                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), filtro, ordinamento, "", this.GetFieldsList(prototype));

            //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            return cmd;
        }

        protected virtual IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina, ref int risultati)
        {
            return null;
        }


        #endregion

        #region EXECUTE SQL


        public IEnumerable GetItemsFromCommand(SupportCreaElemento prototype, IDbCommand cmd, IDbTransaction tx)
        {
            IDataReader reader = null;
            IDbConnection conn = null;
            if (tx == null)
            {
                conn = this.GetConnection();

                if (cmd == null)
                    return null;

                cmd.Connection = conn;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;


                if (cmd == null)
                    return null;

                cmd.Connection = conn;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                reader = cmd.ExecuteReader();

                ((IDisposable)cmd).Dispose();
            }
            return (IEnumerable)reader;
        }

        public IEnumerable GetItemsFromSQL(SupportCreaElemento prototype, string comandoSQL, IDbTransaction tx)
        {
            IDataReader reader = null;
            IDbCommand cmd = null;
            IDbConnection conn = null;
            if (tx == null)
            {
                conn = this.GetConnection();

                cmd = GetItemsCommand(conn, prototype, comandoSQL);

                if (cmd == null)
                    return null;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;

                cmd = GetItemsCommand(conn, prototype, comandoSQL);

                if (cmd == null)
                    return null;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                reader = cmd.ExecuteReader();

                ((IDisposable)cmd).Dispose();
            }
            return (IEnumerable)reader;
        }

        public IEnumerable GetItemsFromSQL(SupportCreaElemento prototype, string comandoSQL, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx)
        {
            IDataReader reader = null;
            IDbCommand cmd = null;
            IDbConnection conn = null;
            if (tx == null)
            {

                conn = this.GetConnection();

                cmd = GetItemsCommand(conn, prototype, comandoSQL, elementiPerPagina, pagina, ref risultati);

                if (cmd == null)
                    return null;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;

                cmd = GetItemsCommand(conn, prototype, comandoSQL, elementiPerPagina, pagina, ref risultati);

                if (cmd == null)
                    return null;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                reader = cmd.ExecuteReader();

                ((IDisposable)cmd).Dispose();
            }
            return (IEnumerable)reader;
        }

        protected virtual IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, string comandoSQL)
        {
            IDbCommand cmd = conn.CreateCommand();

            cmd.CommandText = comandoSQL;

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            return cmd;
        }

        protected virtual IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, string comandoSQL, int elementiPerPagina, int pagina, ref int risultati)
        {
            return null;
        }

        #endregion

        #region COUNT ITEMS

        public int CountItems(SupportCreaElemento prototype, IFilter filtro, IDbTransaction tx)
        {
            IDbCommand cmd = null;
            IDbConnection conn = null;
            int results = 0;

            if (tx == null)
            {
                conn = this.GetConnection();

                cmd = CountItemsCommand(conn, prototype, filtro.ToString());
                ActivateBindByName(cmd);
                FillQueryParameter(filtro, cmd);

                if (cmd == null)
                    throw new Exception("Impossibile creare il comando");

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
                results = Convert.ToInt32(cmd.ExecuteScalar());

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;

                cmd = CountItemsCommand(conn, prototype, filtro.ToString());
                ActivateBindByName(cmd);
                FillQueryParameter(filtro, cmd);

                if (cmd == null)
                    throw new Exception("Impossibile creare il comando");

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
                results = Convert.ToInt32(cmd.ExecuteScalar());

                ((IDisposable)cmd).Dispose();
            }

            return results;
        }

        protected virtual IDbCommand CountItemsCommand(IDbConnection conn, SupportCreaElemento prototype, string filtro)
        {
            DataItem item = new DataItem(prototype);
            int fieldCount = item.FieldCount;
            List<String> PKfields = new List<String>();
            System.Reflection.PropertyInfo[] props = prototype.GetType().GetProperties();
            for (int idx = 0; idx < fieldCount; idx++)
            {
                // identifico le primary key
                if (props[idx].CanWrite && this.IsMappedField(props[idx]))
                {
                    if (this.IsPrimaryKey(props[idx]) == true)
                    {
                        PKfields.Add(item.GetName(idx));
                    }
                }
            }

            //se non ho una unica primarykey uso il primo campo
            if (PKfields.Count != 1)
            {
                PKfields.Clear();
                PKfields.Add(item.GetName(0));
            }

            string cmdText = "Select Count(" + string.Join(",", PKfields.ToArray()) + ") From {0}{1}";

            IDbCommand cmd = conn.CreateCommand();
            if (filtro != null && filtro != "")
            {
                filtro = " Where " + filtro;
                filtro = filtro.Replace("*", JollyCharacter);
            }
            cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), filtro);
            //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            return cmd;
        }

        public int CountItemsFromSQL(string comandoSQL, IDbTransaction tx)
        {
            IDbCommand cmd = null;
            IDbConnection conn = null;
            int results = 0;

            if (tx == null)
            {
                conn = this.GetConnection();

                cmd = CountItemsCommand(conn, comandoSQL);

                if (cmd == null)
                    throw new Exception("Impossibile creare il comando");

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                System.Diagnostics.Debug.WriteLine("CountItemsFromSQL Command = " + cmd.CommandText);
                results = Convert.ToInt32(cmd.ExecuteScalar());

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;

                cmd = CountItemsCommand(conn, comandoSQL);

                if (cmd == null)
                    throw new Exception("Impossibile creare il comando");

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                System.Diagnostics.Debug.WriteLine("CountItemsFromSQL Command = " + cmd.CommandText);
                results = Convert.ToInt32(cmd.ExecuteScalar());

                ((IDisposable)cmd).Dispose();
            }

            return results;
        }

        protected virtual IDbCommand CountItemsCommand(IDbConnection conn, string comandoSQL)
        {
            IDbCommand cmd = conn.CreateCommand();

            string cmdText = "Select Count(0) as cnt From ({0}) sub";
            cmd.CommandText = string.Format(cmdText, comandoSQL);
            System.Diagnostics.Debug.WriteLine("CountItemsFromSQL Command = " + cmd.CommandText);

            return cmd;
        }

        #endregion

        #region STORED PROCEDURE

        protected virtual IDbCommand StoredProcedureCommand(IDbConnection conn, IFrameworkStoredProcedure proc)
        {
            IDbCommand cmd = conn.CreateCommand();

            if (proc.ReturnTable)
            {
                cmd.CommandType = CommandType.Text;
                List<string> realInParams = new List<string>();
                foreach (FrameworkStoredProcedureParameter Fparam in proc.Input.Values)
                {
                    if (!Fparam.ParameterName.StartsWith("@") && !Fparam.ParameterName.StartsWith("IN_"))
                        continue;

                    realInParams.Add(Fparam.ParameterName);
                }
                cmd.CommandText = string.Format("SELECT * FROM {0}({1})", proc.Nome, string.Join(",", realInParams.ToArray()));
            }
            else
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = proc.Nome;
            }

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            foreach (FrameworkStoredProcedureParameter Fparam in proc.Input.Values)
            {
                if (!Fparam.ParameterName.StartsWith("@") && !Fparam.ParameterName.StartsWith("IN_"))
                    continue;

                IDbDataParameter param = cmd.CreateParameter();
                cmd.Parameters.Add(param);

                param.ParameterName = Fparam.ParameterName;

                param.DbType = Fparam.Tipo;
                param.Direction = Fparam.Direzione;

                if (Fparam.Valore == null)
                {
                    param.Value = System.DBNull.Value;
                }
                else
                {
                    if (param.DbType == DbType.DateTime)
                    {
                        if (!AllowDateTimeField(Fparam))
                            Fparam.Valore = ((DateTime)Fparam.Valore).Date;
                        //if (CheckParamDateTime(Fparam.ParameterName) == false)
                        //    Fparam.Valore = ((DateTime)Fparam.Valore).Date;
                    }
                    param.Value = Fparam.Valore;
                }

                System.Diagnostics.Debug.WriteLine("PROCEDURE param name = " + param.ParameterName + "  -  param value = " + param.Value.ToString());
            }


            foreach (FrameworkStoredProcedureParameter Fparam in proc.Output.Values)
            {
                IDbDataParameter param = cmd.CreateParameter();
                cmd.Parameters.Add(param);
                param.ParameterName = Fparam.ParameterName;
                param.DbType = Fparam.Tipo;
                if (param.DbType == DbType.String)
                {
                    param.Size = Fparam.Lunghezza;
                }
                param.Direction = Fparam.Direzione;
            }

            return cmd;
        }

        #endregion

        #region TRANSAZIONE

        public IEnumerable LaunchStoredProcedure(IFrameworkStoredProcedure proc, IDbTransaction tx, out object result)
        {
            result = null;
            IDbConnection conn = this.GetConnection();
            IDataReader reader = null;

            if (tx == null)
                throw new Exception("Impossibile eseguire l'operazione su di una transazione nulla");

            IDbCommand cmd = StoredProcedureCommand(conn, proc);
            cmd.Transaction = tx;

            if (!proc.ReturnTable)
            {
                result = cmd.ExecuteScalar();
                //proc.Output.Add("Output", new FrameworkStoredProcedureParameter("Output", ParameterDirection.Output, DbType.String, res.ToString()));
            }
            else
            {

                if (tx == null)
                {
                    reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                else
                {
                    cmd.Transaction = tx;
                    reader = cmd.ExecuteReader();
                }
            }
            //recupero le informazioni dei valori di output per la procedura
            foreach (FrameworkStoredProcedureParameter Fparam in proc.Output.Values)
            {
                Fparam.Valore = ((IDbDataParameter)cmd.Parameters[Fparam.ParameterName]).Value;
            }

            cmd.Dispose();
            return (IEnumerable)reader;
        }


        public virtual IDbTransaction OpenTransaction()
        {
            IDbConnection conn = this.GetConnection();

            if (conn.State != ConnectionState.Open)
                conn.Open();

            return conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public virtual void CloseTransaction(IDbTransaction tx, bool commit)
        {
            try
            {
                if (commit == true)
                    tx.Commit();
                else
                    tx.Rollback();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                this.Close();
                throw;
            }

            this.Close();
        }

        #endregion

        #region SQL STRING


        public virtual string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina)
        {
            return "";
        }

        public virtual string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento)
        {
            string cmdText = "Select {4}{3} From {0}{1}{2}";

            string f = filtro.ToNonParametricString();
            if (f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            if (this.GetAggiunteSelect(prototype) != null)
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "," + this.GetAggiunteSelect(prototype), this.GetFieldsList(prototype));
            else
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "", this.GetFieldsList(prototype));

            return cmdText;
        }

        #endregion

        #region INSERT ITEM

        protected virtual IDbCommand InsertItemCommand(IDbConnection conn, IDataRecord item, Type itemType, ref long generatedID, out Boolean usesAutonumberField)
        {
            string sqlMask = "Insert Into {0}({1}) Values({2})";
            int fieldCount = item.FieldCount;
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);
            ArrayList fields = new ArrayList();
            ArrayList valPlaceHolders = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();
            List<string> parametersEvidence = new List<string>();

            usesAutonumberField = false;

            IDbCommand cmd = conn.CreateCommand();


            SetStandardDate("create", item, props);


            for (int idx = 0; idx < fieldCount; idx++)
            {
                int idx2 = idx;
                // nella gestione SQL entrano solo le properties scrivibili e marcate con l'attributo MappedField
                if (props[idx2].CanWrite && this.IsMappedField(props[idx2]))
                {
                    //TODO: verificare che ordine properties combaci con datarecord 
                    //porre rimedio calcolando indice secondario
                    if (props[idx2].Name != item.GetName(idx))
                    {
                        for (int i = 0; i < props.Length; i++)
                        {
                            if (props[i].Name == item.GetName(idx))
                            {
                                idx2 = i;
                                break;
                            }
                        }
                        //throw new Exception("Ordine errato dati rispetto alle properties.");
                    }


                    valPlaceHolders.Add(parameterPlaceHolder + idx.ToString());
                    fields.Add(item.GetName(idx));

                    object val = item.GetValue(idx);

                    if (val == null)
                        val = DBNull.Value;

                    if (val != null && this.IsNullableForeignKey(props[idx2]) == true)
                    {
                        if (val.GetType().Name == "Decimal" && (decimal)val <= 0)
                            val = DBNull.Value;

                        if (val.GetType().Name == "String" && val.ToString() == "")
                            val = DBNull.Value;
                    }


                    if (val.GetType().Name == "Boolean")
                    {
                        val = TranslateBooleanValue(val);
                    }

                    if (item.GetName(idx) == "CDATE" || item.GetName(idx) == "MDATE")
                    {
                        if (val == null || (DateTime)val == DateTime.MinValue)
                            val = DateTime.Now;
                    }

                    if (val.GetType().Name == "DateTime")
                    {
                        if (!AllowDateTimeField(props[idx2]))
                            val = ((DateTime)val).Date;
                        //if (CheckParamDateTime(item.GetName(idx)) == false)
                        //    val = ((DateTime)val).Date;

                        if (((DateTime)val) == DateTime.MinValue)
                        {
                            val = DBNull.Value;
                        }
                    }

                    IDbDataParameter param = cmd.CreateParameter();
                    cmd.Parameters.Add(param);
                    param.ParameterName = parameterPlaceHolder + idx.ToString();

                    if (val.GetType().Name == "String")
                    {
                        int textLimit = this.SizeLimitedField(props[idx2]);
                        if (textLimit != 0 && val.ToString().Length > textLimit)
                        {
                            throw new Exception(string.Format("Il testo per {0} supera il limite di caratteri consentiti ({1})", item.GetName(idx), textLimit.ToString()));
                            //val = val.ToString().Substring(1, this.SizeLimitedField(props[idx]));
                        }

                        if (val.ToString().Length > 2000)
                            SetLongStringParamType(param);
                    }

                    if (this.IsPrimaryKey(props[idx2]) == true)
                    {
                        try
                        {
                            //se la conversione da errore non si tratta di un valore numerico gestibile via sequence
                            Decimal tmp = Convert.ToDecimal(val);

                            string sequenceName = this.GetSequenceName(itemType);

                            if (tmp == 0)
                            {
                                //estraggo il prossimo ID dalla sequence se indicato
                                if (!string.IsNullOrEmpty(sequenceName))
                                {
                                    System.Diagnostics.Debug.WriteLine("ORACLE Sequence " + sequenceName + " for field " + props[idx2]);
                                    long nuovoID = ProssimoID(sequenceName);
                                    if (nuovoID != 0)
                                    {
                                        val = (object)nuovoID;
                                        generatedID = nuovoID;
                                    }
                                    else
                                    {
                                        //l'id ? un contatore...
                                        fields.RemoveAt(idx);
                                        valPlaceHolders.RemoveAt(idx);
                                        cmd.Parameters.RemoveAt(idx);
                                        //recuperare l'id
                                        usesAutonumberField = true;
                                    }
                                }
                                else
                                {
                                    //l'id ? un contatore...
                                    fields.RemoveAt(idx);
                                    valPlaceHolders.RemoveAt(idx);
                                    cmd.Parameters.RemoveAt(idx);
                                    //recuperare l'id
                                    usesAutonumberField = true;
                                }
                            }
                            //else if (tmp < 0)
                            //{
                            //    //l'id ? un contatore...
                            //    fields.RemoveAt(idx);
                            //    valPlaceHolders.RemoveAt(idx);
                            //    cmd.Parameters.RemoveAt(idx);
                            //    //recuperare l'id
                            //    usesAutonumberField = true;
                            //}
                            else
                            {
                                generatedID = (int)tmp;
                            }
                        }
                        catch
                        {
                        }

                    }

                    param.Value = val;

                    if (param.Value == DBNull.Value)
                    {
                        parametersEvidence.Add("|\tParam = " + param.ParameterName + " -> " + "NULL");
                    }
                    else
                    {
                        parametersEvidence.Add("|\tParam = " + param.ParameterName + " -> " + param.Value.ToString());
                    }
                }
            }


            cmd.CommandText = String.Format(
                    sqlMask,
                    tableName,
                    String.Join(",", (String[])fields.ToArray(typeof(string))),
                    String.Join(",", (String[])valPlaceHolders.ToArray(typeof(string)))
                );

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            foreach (string s in parametersEvidence)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }
            return cmd;
        }


        protected virtual string GetSequenceName(Type itemType)
        {
            try
            {
                IFrameworkObject obj = (IFrameworkObject)Type.GetType(itemType.AssemblyQualifiedName).GetConstructor(new Type[] { }).Invoke(null);
                return obj.SequenceName();
            }
            catch
            {
                return null;
            }
        }

        public virtual long ProssimoID(string NomeSequenza)
        {
            return 0;
        }

        public Exception InsertItem(IDataRecord item, Type itemType, IDbTransaction tx, ref long generatedID)
        {
            IDbConnection conn = this.connection;

            generatedID = 0;

            IDbCommand cmd;
            bool usesAutonumberField;
            try
            {
                cmd = InsertItemCommand(conn, item, itemType, ref generatedID, out usesAutonumberField);
            }
            catch (Exception ex)
            {
                return ex;
            }
            cmd.Transaction = tx;

            Exception result = null;
            try
            {
                int res = cmd.ExecuteNonQuery();

                if (usesAutonumberField == true)
                {
                    //estraggo l'id autogenerato
                    cmd.CommandText = SelectIdentity(GetSequenceName(itemType));
                    if (cmd.CommandText != string.Empty)
                        generatedID = Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                result = ex;
            }
            cmd.Dispose();

            return result;
        }

        #endregion

        #region UPDATE ITEM

        protected virtual IDbCommand UpdateItemCommand(IDbConnection conn, IDataRecord item, Type itemType, IFilter filter = null)
        {
            string sqlMask = "Update {0} set {1} where {2}{3}";
            int fieldCount = item.FieldCount;
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);
            ArrayList fields = new ArrayList();
            ArrayList PKfields = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();
            IList<string> itemPropsChanged = this.GetPropertiesChanged(item);

            IDbCommand cmd = conn.CreateCommand();

            List<IDbDataParameter> pkParams = new List<IDbDataParameter>();

            SetStandardDate("update", item, props);


            for (int idx = 0; idx < fieldCount; idx++)
            {
                // nella gestione SQL entrano solo le properties scrivibili e marcate con l'attributo MappedField
                if (props[idx].CanWrite && this.IsMappedField(props[idx]))
                {
                    //TODO: verificare che ordine properties combaci con datarecord 
                    //porre rimedio calcolando indice secondario
                    if (props[idx].Name != item.GetName(idx))
                        throw new Exception("Ordine errato dati rispetto alle properties.");

                    string paramName = parameterPlaceHolder + idx.ToString();
                    if (this.IsPrimaryKey(props[idx]) == true)
                    {
                        PKfields.Add(item.GetName(idx) + " = " + paramName);
                    }
                    else
                    {
                        if (itemPropsChanged.Contains(item.GetName(idx).ToLower()) == false)
                        {
                            //property not changed
                            continue;
                        }

                        fields.Add(item.GetName(idx) + " = " + paramName);
                    }

                    object val = item.GetValue(idx);

                    if (val == null)
                        val = DBNull.Value;


                    if (val != null && this.IsNullableForeignKey(props[idx]) == true)
                    {
                        if (val.GetType().Name == "Decimal" && (decimal)val <= 0)
                            val = DBNull.Value;

                        if (val.GetType().Name == "String" && val.ToString() == "")
                            val = DBNull.Value;
                    }

                    if (val.GetType().Name == "Boolean")
                    {
                        val = TranslateBooleanValue(val);
                    }

                    if (val.GetType().Name == "DateTime")
                    {
                        if (!AllowDateTimeField(props[idx]))
                            val = ((DateTime)val).Date;
                        //if (CheckParamDateTime(item.GetName(idx)) == false)
                        //    val = ((DateTime)val).Date;

                        if (((DateTime)val) == DateTime.MinValue)
                        {
                            val = DBNull.Value;
                        }
                    }


                    IDbDataParameter param = cmd.CreateParameter();
                    param.ParameterName = paramName;
                    if (this.IsPrimaryKey(props[idx]) == true)
                        pkParams.Add(param); //aggiungo alla lista dei parametri PK che devono essere accodati agli altri
                    else
                        cmd.Parameters.Add(param);

                    if (val.GetType().Name == "String")
                    {
                        param.DbType = DbType.String;
                        param.Direction = ParameterDirection.Input;
                        param.SourceColumn = item.GetName(idx);
                        int textLimit = this.SizeLimitedField(props[idx]);
                        if (textLimit != 0 && val.ToString().Length > textLimit)
                        {
                            throw new Exception(string.Format("Il testo per {0} supera il limite di caratteri consentiti ({1})", item.GetName(idx), textLimit.ToString()));
                            //val = val.ToString().Substring(1, this.SizeLimitedField(props[idx]));
                        }

                        if (val.ToString().Length > 2000)
                            SetLongStringParamType(param);
                    }

                    param.Value = val;
                }
            }

            if (fields.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("ORACLE Command = Nessun dato modificato");

                cmd.CommandText = "";
                //throw new Exception("Nessun dato modificato");
            }
            else
            {
                foreach (IDbDataParameter par in pkParams)
                {
                    cmd.Parameters.Add(par);
                }

                System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

                cmd.CommandText = String.Format(
                        sqlMask,
                        tableName,
                        String.Join(",", (String[])fields.ToArray(typeof(string))),
                        String.Join(" AND ", (String[])PKfields.ToArray(typeof(string))),
                        filter != null ? filter.ToNonParametricString() : "");
            }

            return cmd;
        }

        protected static void SetStandardDate(string operation, IDataRecord item, System.Reflection.PropertyInfo[] props)
        {
            Type attType = null;
            String baseNamingConvention = "";
            switch (operation.ToLower())
            {
                case "create":
                    attType = typeof(DateCreation);
                    baseNamingConvention = "DATAINSERIMENTO";
                    break;
                case "update":
                    attType = typeof(DateLastModified);
                    baseNamingConvention = "DATAMODIFICA,MDATE";
                    break;
                default:
                    //do nothing
                    return;
            }

            foreach (PropertyInfo prop in props)
            {
                if (baseNamingConvention.Contains(prop.Name.ToUpper()) ||
                    prop.GetCustomAttributes(attType, true).Length != 0)
                {
                    //Creation or Modification dates, set current if null
                    var date = prop.GetValue(((DataItem)(item)).GetItem, null);
                    if (date == null)
                    {
                        prop.SetValue(((DataItem)(item)).GetItem, DateTime.Now, null);
                    }

                    break;
                }
            }
        }

        public Exception UpdateItem(IDataRecord item, Type itemType, IDbTransaction tx, out long recordCount, IFilter filter = null)
        {
            IDbConnection conn = this.connection;
            IDbCommand cmd;

            recordCount = 0;

            try
            {
                cmd = UpdateItemCommand(conn, item, itemType);
            }
            catch (Exception ex)
            {
                return ex;
            }

            cmd.Transaction = tx;

            Exception result = null;

            if (cmd.CommandText.Length > 0)
            {
                try
                {
                    recordCount = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    result = ex;
                }
            }

            cmd.Dispose();

            return result;
        }

        #endregion

        #region DELETE ITEM

        protected virtual IDbCommand DeleteItemCommand(IDbConnection conn, IDataRecord item, Type itemType)
        {
            string sqlMask = "Delete from {0} where {1}";
            int fieldCount = item.FieldCount;
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);
            ArrayList PKfields = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();

            IDbCommand cmd = conn.CreateCommand();

            for (int idx = 0; idx < fieldCount; idx++)
            {
                string paramName = parameterPlaceHolder + idx.ToString();
                if (this.IsPrimaryKey(props[idx]) == true)
                {
                    PKfields.Add(item.GetName(idx) + " = " + paramName);

                    object val = item.GetValue(idx);

                    if (val == null)
                        val = DBNull.Value;

                    IDbDataParameter param = cmd.CreateParameter();
                    cmd.Parameters.Add(param);
                    param.ParameterName = paramName;
                    param.Value = val;
                }
            }

            cmd.CommandText = String.Format(
                    sqlMask,
                    tableName,
                    String.Join(" AND ", (String[])PKfields.ToArray(typeof(string)))
                );

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            return cmd;
        }

        public Exception DeleteItem(IDataRecord item, Type itemType, IDbTransaction tx, out long recordCount)
        {
            recordCount = 0;

            IDbConnection conn = this.connection;

            IDbCommand cmd = DeleteItemCommand(conn, item, itemType);

            cmd.Transaction = tx;


            Exception result = null;
            try
            {
                recordCount = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = ex;
            }

            cmd.Dispose();

            return result;
        }

        #endregion

        #region DELETE WHERE...

        protected virtual IDbCommand DeleteItemCommand(IDbConnection conn, Type itemType, string filter)
        {
            string sqlMask = "Delete from {0} where {1}";
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);

            IDbCommand cmd = conn.CreateCommand();

            cmd.CommandText = String.Format(
                        sqlMask,
                        tableName,
                        filter
                        );

            return cmd;
        }

        public Exception DeleteItem(Type itemType, IFilter filter, IDbTransaction tx, out long recordCount)
        {
            recordCount = 0;
            IDbConnection conn = this.connection;

            IDbCommand cmd = DeleteItemCommand(conn, itemType, filter.ToString());
            ActivateBindByName(cmd);
            FillQueryParameter(filter, cmd);
            cmd.Transaction = tx;

            Exception result = null;
            try
            {
                recordCount = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = ex;
            }

            cmd.Dispose();

            return result;
        }

        #endregion



        #region GESTIONE BLOB

        protected virtual IDbCommand ReadBlobCommand(IDbConnection conn, IFrameworkObjectWithBlob item, string blobField)
        {
            string sqlMask = "SELECT {0} from {1} where {2}";
            Type itemType = item.GetType();
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);
            ArrayList PKfields = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();

            IDbCommand cmd = conn.CreateCommand();
            int idx = 0;

            foreach (PropertyInfo prop in props)
            {
                if (prop.CanWrite)
                {
                    if (this.IsPrimaryKey(prop) == true)
                    {
                        string paramName = parameterPlaceHolder + idx.ToString();

                        PKfields.Add(prop.Name + " = " + paramName);

                        object val = prop.GetValue(item, null);

                        if (val == null)
                            val = DBNull.Value;

                        IDbDataParameter param = cmd.CreateParameter();
                        cmd.Parameters.Add(param);
                        param.ParameterName = paramName;
                        param.Value = val;

                        idx++;
                    }
                }
            }

            cmd.CommandText = String.Format(
                    sqlMask,
                    blobField,
                    tableName,
                    String.Join(" AND ", (String[])PKfields.ToArray(typeof(string)))
                );

            return cmd;
        }

        public virtual byte[] ReadBlob(IFrameworkObjectWithBlob item, string blobField, IDbTransaction tx)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual IDbCommand WriteBlobCommand(IDbConnection conn, IFrameworkObjectWithBlob item, string blobField, byte[] blobData)
        {
            string sqlMask = "Update {0} set {1} where {2}";
            Type itemType = item.GetType();
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);
            ArrayList PKfields = new ArrayList();
            ArrayList fields = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();
            string paramName = string.Empty;

            IDbCommand cmd = conn.CreateCommand();
            IDbDataParameter param;

            int idx = 1;

            foreach (PropertyInfo prop in props)
            {
                if (prop.CanWrite)
                {
                    if (this.IsPrimaryKey(prop) == true)
                    {
                        paramName = parameterPlaceHolder + idx.ToString();

                        PKfields.Add(prop.Name + " = " + paramName);

                        object val = prop.GetValue(item, null);

                        if (val == null)
                            val = DBNull.Value;

                        param = cmd.CreateParameter();
                        cmd.Parameters.Add(param);
                        param.ParameterName = paramName;
                        param.Value = val;

                        idx++;
                    }
                }
            }


            paramName = parameterPlaceHolder + "0";
            fields.Add(blobField + " = " + paramName);
            param = cmd.CreateParameter();
            cmd.Parameters.Add(param);
            param.DbType = DbType.Binary;
            param.ParameterName = paramName;
            param.Value = blobData;


            cmd.CommandText = String.Format(
                    sqlMask,
                    tableName,
                    String.Join(",", (String[])fields.ToArray(typeof(string))),
                    String.Join(" AND ", (String[])PKfields.ToArray(typeof(string)))
                );

            return cmd;
        }

        public virtual Exception WriteBlob(IFrameworkObjectWithBlob item, string blobField, byte[] blobData, IDbTransaction tx)
        {
            IDbConnection conn = this.connection;
            IDbCommand cmd;

            try
            {
                cmd = WriteBlobCommand(conn, item, blobField, blobData);
            }
            catch (Exception ex)
            {
                return ex;
            }
            cmd.Transaction = tx;

            Exception result = null;
            try
            {
                int res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = ex;
            }
            cmd.Dispose();

            return result;
        }

        #endregion


        #region GESTIONE CLOB

        protected virtual IDbCommand WriteClobCommand(IDbConnection conn, IFrameworkObjectWithBlob item, string blobField, string clobData)
        {
            string sqlMask = "Update {0} set {1} where {2}";
            Type itemType = item.GetType();
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);
            ArrayList PKfields = new ArrayList();
            ArrayList fields = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();
            string paramName = string.Empty;

            IDbCommand cmd = conn.CreateCommand();
            IDbDataParameter param;

            int idx = 1;

            foreach (PropertyInfo prop in props)
            {
                if (prop.CanWrite)
                {
                    if (this.IsPrimaryKey(prop) == true)
                    {
                        paramName = parameterPlaceHolder + idx.ToString();

                        PKfields.Add(prop.Name + " = " + paramName);

                        object val = prop.GetValue(item, null);

                        if (val == null)
                            val = DBNull.Value;

                        param = cmd.CreateParameter();
                        cmd.Parameters.Add(param);
                        param.ParameterName = paramName;
                        param.Value = val;

                        idx++;
                    }
                }
            }


            paramName = parameterPlaceHolder + "0";
            fields.Add(blobField + " = " + paramName);
            param = cmd.CreateParameter();
            cmd.Parameters.Add(param);
            param.DbType = DbType.AnsiString;
            param.ParameterName = paramName;
            param.Value = clobData;


            cmd.CommandText = String.Format(
                    sqlMask,
                    tableName,
                    String.Join(",", (String[])fields.ToArray(typeof(string))),
                    String.Join(" AND ", (String[])PKfields.ToArray(typeof(string)))
                );

            return cmd;
        }

        public virtual Exception WriteClob(IFrameworkObjectWithBlob item, string clobField, string clobData, IDbTransaction tx)
        {
            IDbConnection conn = this.connection;
            IDbCommand cmd;

            try
            {
                cmd = WriteClobCommand(conn, item, clobField, clobData);
                ActivateBindByName(cmd);
            }
            catch (Exception ex)
            {
                return ex;
            }
            cmd.Transaction = tx;

            Exception result = null;
            try
            {
                int res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = ex;
            }
            cmd.Dispose();

            return result;
        }


        #endregion


        #region GET ITEMS FROM SELECTION

        public string[] GetItemsField(SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, IDbTransaction tx)
        {
            IEnumerable reader = null;
            IDbCommand cmd = null;
            IDbConnection conn = null;
            List<string> results = new List<string>();

            if (tx == null)
            {
                conn = this.GetConnection();

                cmd = GetItemsCommand(conn, prototype, filtro.ToString(), ordinamento, field);
                ActivateBindByName(cmd);
                FillQueryParameter(filtro, cmd);

                if (cmd == null)
                    return new string[0];

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                reader = (IEnumerable)cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            else
            {
                conn = this.connection;

                cmd = GetItemsCommand(conn, prototype, filtro.ToString(), ordinamento, field);
                ActivateBindByName(cmd);
                FillQueryParameter(filtro, cmd);

                if (cmd == null)
                    return new string[0];

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                reader = (IEnumerable)cmd.ExecuteReader();
            }
            bool hasRows = true;
            try
            {
                hasRows = ((System.Data.Common.DbDataReader)reader).HasRows;
            }
            catch { }
            if (reader is System.Data.Common.DbDataReader && hasRows == false)
            {
                ((IDisposable)cmd).Dispose();
                ((IDataReader)reader).Close();
                ((IDataReader)reader).Dispose();
                return results.ToArray();
            }


            string value = string.Empty;
            foreach (IDataRecord item in reader)
            {

                if (item[0].GetType().Name == "System.String")
                {
                    value = string.Format("'{0}'", item[0]);
                }
                else
                {
                    value = item[0].ToString();
                }

                if (!results.Contains(value))
                    results.Add(value);
            }

            ((IDisposable)cmd).Dispose();
            ((IDataReader)reader).Close();
            ((IDataReader)reader).Dispose();

            return results.ToArray();
        }

        protected virtual IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, string filtro, string ordinamento, string field)
        {
            IDbCommand cmd = conn.CreateCommand();

            string cmdText = "Select {3} From {0}{1}{2}";

            if (filtro != null && filtro != "")
            {
                filtro = " Where " + filtro;
                filtro = filtro.Replace("*", JollyCharacter);
            }

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), filtro, ordinamento, field);

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            return cmd;
        }


        public string[] GetItemsField(SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, int elementiPerPagina, int pagina, ref int risultati, IDbTransaction tx)
        {
            IEnumerable reader = null;
            IDbCommand cmd = null;
            IDbConnection conn = null;
            List<string> results = new List<string>();

            if (tx == null)
            {
                conn = this.GetConnection();

                cmd = GetItemsCommand(conn, prototype, filtro, ordinamento, field, elementiPerPagina, pagina, ref risultati);

                if (cmd == null)
                    return new string[0];

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                reader = (IEnumerable)cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            else
            {
                conn = this.connection;

                cmd = GetItemsCommand(conn, prototype, filtro, ordinamento, field, elementiPerPagina, pagina, ref risultati);

                if (cmd == null)
                    return new string[0];

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                reader = (IEnumerable)cmd.ExecuteReader();
            }

            bool hasRows = true;
            try
            {
                hasRows = ((System.Data.Common.DbDataReader)reader).HasRows;
            }
            catch { }
            if (reader is System.Data.Common.DbDataReader && hasRows == false)
            {
                ((IDisposable)cmd).Dispose();
                ((IDataReader)reader).Close();
                ((IDataReader)reader).Dispose();
                return results.ToArray();
            }

            foreach (IDataRecord item in reader)
            {
                if (item[0].GetType().Name == "System.String")
                {
                    results.Add(string.Format("'{0}'", item[0]));
                }
                else
                {
                    results.Add(item[0].ToString());
                }
            }

            ((IDisposable)cmd).Dispose();
            ((IDataReader)reader).Close();
            ((IDataReader)reader).Dispose();

            return results.ToArray();
        }

        protected virtual IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, int elementiPerPagina, int pagina, ref int risultati)
        {
            return null;
        }

        #endregion

        public void ExecuteCommand(string comandoSQL, IDbTransaction tx)
        {
            IDbCommand cmd = null;
            IDbConnection conn = null;

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + comandoSQL);

            if (tx == null)
            {
                conn = this.GetConnection();
                cmd = conn.CreateCommand();
                cmd.CommandText = comandoSQL;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.ExecuteNonQuery();

                ((IDisposable)cmd).Dispose();
            }
            else
            {
                conn = this.connection;
                cmd = conn.CreateCommand();
                cmd.CommandText = comandoSQL;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Transaction = tx;

                cmd.ExecuteNonQuery();

                ((IDisposable)cmd).Dispose();
            }
        }

    }

}

