#region Using directives

using IT.TnDigit.ORM.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public partial class OracleClientProvider : DataProvider
    {
        protected override System.Data.IDbConnection GetConnection()
        {
            if (this.connection == null || this.connection.ConnectionString == "")
                // leggere la stringa di connessione
                this.connection = CreateConnection();

            return this.connection;
        }

        protected override void ActivateBindByName(IDbCommand cmd)
        {
            ((OracleCommand)cmd).BindByName = true;
        }


        public override IDbConnection CreateConnection()
        {
            return new OracleConnection(this.connectionString);
        }

        public override string parameterPlaceHolder
        {
            get
            {
                return ":P";
            }
        }

        protected override void SetLongStringParamType(IDbDataParameter param)
        {
            ((OracleParameter)param).OracleDbType = OracleDbType.Clob;
        }


        protected override IDbCommand StoredProcedureCommand(IDbConnection conn, IFrameworkStoredProcedure proc)
        {
            OracleCommand cmd = ((OracleConnection)conn).CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.CommandText = proc.Nome;

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            OracleParameter paramOrc;
            IDbDataParameter param;
            foreach (FrameworkStoredProcedureParameter Fparam in proc.Input.Values)
            {
                param = cmd.CreateParameter();
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
                    }
                    param.Value = Fparam.Valore;
                }
                System.Diagnostics.Debug.WriteLine("PROCEDURE param name = " + param.ParameterName + "  -  param value = " + param.Value.ToString());
            }


            foreach (FrameworkStoredProcedureParameter Fparam in proc.Output.Values)
            {
                if (Fparam.Tipo == DbType.Binary)
                {
                    paramOrc = cmd.CreateParameter();
                    cmd.Parameters.Add(paramOrc);
                    paramOrc.ParameterName = Fparam.ParameterName;
                    paramOrc.OracleDbType = OracleDbType.Blob;
                    paramOrc.Direction = Fparam.Direzione;
                    paramOrc.Value = Fparam.Valore;

                }
                else
                {
                    param = cmd.CreateParameter();
                    cmd.Parameters.Add(param);
                    param.ParameterName = Fparam.ParameterName;
                    param.DbType = Fparam.Tipo;
                    if (param.DbType == DbType.String)
                    {
                        param.Size = Fparam.Lunghezza;
                    }
                    param.Direction = Fparam.Direzione;
                }
            }

            return cmd;
        }

        public override string DateFilter(string data)
        {
            return string.Format("TO_DATE('{0}','dd/MM/yyyy')", data);
        }

        public override string DateTimeFilter(string data)
        {
            return string.Format("TO_DATE('{0}','dd/MM/yyyy hh24:mi:ss')", data);
        }

        public override string DateFilterTruncateField(string campo)
        {
            //return campo;
            if (campo.Contains("{0}") == false)
                return campo;

            return campo.Replace("{0}", "TRUNC({0},'DDD')");
        }


        protected override int SizeLimitedField(PropertyInfo prop)
        {
            Object[] attributes = prop.GetCustomAttributes(true);
            foreach (Object att in attributes)
            {
                if ((Attribute)att is MaxLength)
                {
                    return ((MaxLength)att).Max;
                }
            }
            return 0;
        }

        public override string JollyCharacter
        {
            get
            {
                return "%";
            }
        }

        #region GET ITEMS PAGINATI

        protected override IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina, ref int risultati)
        {
            risultati = this.CountItems(prototype, filtro, null);

            IDbCommand cmd = conn.CreateCommand();
            string f = filtro.ToString();
            if (f != null && f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }
            ActivateBindByName(cmd);
            FillQueryParameter(filtro, cmd);

            string cmdText = "";

            cmdText = "Select * from (select TB.*,rownum as num FROM (select {0}.*{3} From {0}{1}{2}) TB ) where num>{4} and num<={5}";

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            if (this.GetAggiunteSelect(prototype) != null)
                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "," + this.GetAggiunteSelect(prototype), ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));
            else
                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "", ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));

            //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            return cmd;
        }

        public override string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina)
        {
            string cmdText = "Select * from (select {0}.*{3},rownum as num From {0}{1}{2}) where num>{4} and num<={5}";

            string f = filtro.ToNonParametricString();
            if (f != null && f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            if (this.GetAggiunteSelect(prototype) != null)
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "," + this.GetAggiunteSelect(prototype), ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));
            else
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "", ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));

            return cmdText;
        }

        protected override IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, int elementiPerPagina, int pagina, ref int risultati)
        {
            IDbCommand cmd = conn.CreateCommand();
            string cmdText = "";
            string f = filtro.ToString();
            if (f != null && f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }
            ActivateBindByName(cmd);
            FillQueryParameter(filtro, cmd);

            cmdText = "Select Count({0}) From (Select DISTINCT {0} from {1}{2})";

            cmd.CommandText = string.Format(cmdText, field, this.GetTableName(prototype), filtro);

            System.Diagnostics.Debug.WriteLine("ORACLE cmd.CommandText = " + cmd.CommandText);

            System.Diagnostics.Debug.WriteLine("ORACLE Conn string = " + conn.ConnectionString);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.CommandTimeout = 180;

            int contatore = 0;

            try
            {
                contatore = int.Parse(cmd.ExecuteScalar().ToString());
                risultati = contatore;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                throw ex;
            }


            if (contatore > 0)
            {
                cmdText = "Select * from (select TB.{4},rownum as num FROM (select DISTINCT {4}{3} From {0}{1}{2}) TB ) where num>{5} and num<={6}";
                string orderField = string.Empty;

                if (ordinamento != "" && ordinamento != null)
                {
                    string[] o = ordinamento.Split(',');
                    for (int i = 0; i < o.Length; i++)
                    {
                        o[i] = o[i].Replace(" DESC", "").Replace(" ASC", "");
                    }
                    orderField = "," + string.Join(",", o);

                    ordinamento = " Order by " + ordinamento;
                }

                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, orderField, field, ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));
            }
            else
            {
                return null;
            }

            //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            return cmd;
        }

        #endregion

        #region EXECUTE SQL PAGINATO

        protected override IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, string comandoSQL, int elementiPerPagina, int pagina, ref int risultati)
        {
            string cmdText = "";
            IDbCommand cmd = conn.CreateCommand();

            cmdText = "Select * from ({0}) where num>{1} and num<={2}";

            cmd.CommandText = string.Format(cmdText, comandoSQL, ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            return cmd;
        }

        #endregion

        public override long ProssimoID(string NomeSequenza)
        {
            IDbConnection conn = new OracleConnection(this.connectionString);

            IDbCommand cmCommand = conn.CreateCommand();
            IDataReader drID;
            string strSql;


            strSql = string.Format("SELECT {0}.NEXTVAL FROM DUAL", NomeSequenza);
            cmCommand.CommandText = strSql;
            conn.Open();
            drID = cmCommand.ExecuteReader(CommandBehavior.CloseConnection);

            long longApp = 0;
            while (drID.Read())
            {
                longApp = long.Parse(drID[0].ToString());
            }
            drID.Close();

            return longApp;
        }

        protected override string GetSequenceName(Type itemType)
        {
            string seqName = null;
            try
            {
                IFrameworkObject obj = (IFrameworkObject)Type.GetType(itemType.AssemblyQualifiedName).GetConstructor(new Type[] { }).Invoke(null);
                seqName = obj.SequenceName();

                if (seqName == null)
                {
                    IDbConnection conn = new OracleConnection(this.connectionString);
                    IDbCommand cmCommand = conn.CreateCommand();
                    cmCommand.CommandText = string.Format("Select DATA_DEFAULT from USER_TAB_COLUMNS where TABLE_NAME = '{0}'", obj.TableName());
                    IDataReader drSEQ = cmCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    while (drSEQ.Read())
                    {
                        seqName = drSEQ[0].ToString();
                        break;
                    }
                    drSEQ.Close();
                }

                return seqName;
            }
            catch
            {
                return null;
            }
        }

        protected override string SelectIdentity(string SequenceName)
        {
            if (string.IsNullOrEmpty(SequenceName))
            {
                return string.Empty;
            }
            return string.Format("SELECT {0}.currval from dual", SequenceName);
        }

        #region GESTIONE BLOB

        public override byte[] ReadBlob(IFrameworkObjectWithBlob item, string blobField, IDbTransaction tx)
        {
            IDbConnection conn = null;
            if (tx == null)
            {
                conn = this.GetConnection();
            }
            else
            {
                conn = this.connection;
            }

            IDbCommand cmd = ReadBlobCommand(conn, item, blobField);

            if (cmd == null)
                return null;

            if (conn.State != ConnectionState.Open)
                conn.Open();

            IDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess);

            reader.Read();

            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bin = new System.IO.BinaryWriter(mem);

            long numberOfBytes = 0;
            long startIndex = 0;
            int bufferSize = 2048;
            byte[] buffer = new byte[bufferSize];

            do
            {
                numberOfBytes = reader.GetBytes(0, startIndex, buffer, 0, bufferSize);
                if (numberOfBytes == 0)
                {
                    break;
                }
                bin.Write(buffer, 0, (int)numberOfBytes);
                startIndex += numberOfBytes;
            } while (true);

            bin.Flush();

            if (tx == null)
            {
                conn.Close();
            }

            return mem.ToArray();
        }

        protected override IDbCommand WriteClobCommand(IDbConnection conn, IFrameworkObjectWithBlob item, string blobField, string clobData)
        {
            return base.WriteClobCommand(conn, item, blobField, clobData);
        }
        #endregion

        public override object TranslateBooleanValue(object val)
        {
            if ((bool)val)
                return 1;
            else
                return 0;

        }

        protected override IDbCommand WriteBlobCommand(IDbConnection conn, IFrameworkObjectWithBlob item, string blobField, byte[] blobData)
        {
            string sqlMask = "Update {0} set {1} where {2}";
            Type itemType = item.GetType();
            string typeName = itemType.Name;
            string tableName = this.GetTableName(itemType);

            ArrayList fields = new ArrayList();
            System.Reflection.PropertyInfo[] props = itemType.GetProperties();
            string paramName = string.Empty;

            OracleCommand cmd = conn.CreateCommand() as OracleCommand;

            StringBuilder sbCampiWhere = new StringBuilder();

            foreach (PropertyInfo prop in props)
            {
                if (prop.CanWrite)
                {
                    if (this.IsPrimaryKey(prop) == true)
                    {
                        object val = prop.GetValue(item, null);

                        if (val == null)
                            val = DBNull.Value;

                        if (val != DBNull.Value)
                        {
                            switch (val.GetType().Name)
                            {
                                case "DateTime":
                                    sbCampiWhere.AppendFormat(",{0}={1}", prop.Name, DateFilter(val.ToString()));
                                    break;
                                case "String":
                                    sbCampiWhere.AppendFormat(",{0}='{1}'", prop.Name, val.ToString());
                                    break;
                                default:
                                    sbCampiWhere.AppendFormat(",{0}={1}", prop.Name, val.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            sbCampiWhere.AppendFormat(",{0}=NULL", prop.Name);
                        }
                    }
                }
            }
            sbCampiWhere.Remove(0, 1);


            paramName = parameterPlaceHolder + "0";
            fields.Add(blobField + " = " + paramName);
            cmd.Parameters.Add(paramName, OracleDbType.Blob).Value = blobData;

            cmd.CommandText = String.Format(
                    sqlMask,
                    tableName,
                    String.Join(",", (String[])fields.ToArray(typeof(string))),
                    sbCampiWhere.ToString()
                );

            return cmd;
        }
    }
}



