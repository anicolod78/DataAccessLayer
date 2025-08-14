#region Using directives

using IT.TnDigit.ORM.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Input;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public partial class PostgreSQLProvider : DataProvider
    {
        protected override System.Data.IDbConnection GetConnection()
        {
            if (this.connection == null || this.connection.ConnectionString == "")
                // leggere la stringa di connessione
                this.connection = CreateConnection();

            return this.connection;
        }

        public override IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(this.connectionString);
        }

        public override string parameterPlaceHolder
        {
            get
            {
                return "@P";
            }
        }

        protected override IDbCommand StoredProcedureCommand(IDbConnection conn, IFrameworkStoredProcedure proc)
        {
            NpgsqlCommand cmd = ((NpgsqlConnection)conn).CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = proc.Nome;

            System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);
            NpgsqlParameter paramOrc;
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
            return string.Format("to_date('{0}','DD/MM/YYYY')", data);
        }

        public override string DateTimeFilter(string data)
        {
            return string.Format("to_date('{0}','DD/MM/YYYY HH24:MI:SS')", data);
        }

        public override string DateFilterTruncateField(string campo)
        {
            //return campo;
            if (campo.Contains("{0}") == false)
                return campo;

            return campo.Replace("{0}", "date_trunc('day',{0})");
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

            cmdText = "select TB.* FROM (select {0}.*{3} From {0}{1}{2}) TB offset {4} limit {5}";

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            if (this.GetAggiunteSelect(prototype) != null)
                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "," + this.GetAggiunteSelect(prototype), ((pagina - 1) * elementiPerPagina), elementiPerPagina);
            else
                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "", ((pagina - 1) * elementiPerPagina), elementiPerPagina);

            //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            return cmd;
        }

        public override string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina)
        {
            string cmdText = "select {0}.*{3} From {0}{1}{2} offset {4} limit {5}";

            string f = filtro.ToNonParametricString();
            if (f != null && f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }

            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;

            if (this.GetAggiunteSelect(prototype) != null)
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "," + this.GetAggiunteSelect(prototype), ((pagina - 1) * elementiPerPagina), elementiPerPagina);
            else
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "", ((pagina - 1) * elementiPerPagina), elementiPerPagina);

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
                cmdText = "select TB.{4} FROM (select DISTINCT {4}{3} From {0}{1}{2}) TB offset {5} limit {6}";
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

                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, orderField, field, ((pagina - 1) * elementiPerPagina), elementiPerPagina);
            }
            else
            {
                return null;
            }

            //System.Diagnostics.Debug.WriteLine("ORACLE Command = " + cmd.CommandText);

            return cmd;
        }

        #endregion

        protected override IDbCommand CountItemsCommand(IDbConnection conn, string comandoSQL)
        {
            IDbCommand cmd = conn.CreateCommand();

            string cmdText = "Select Count(0) as cnt From ({0}) as sub";
            cmd.CommandText = string.Format(cmdText, comandoSQL);
            System.Diagnostics.Debug.WriteLine("CountItemsFromSQL Command = " + cmd.CommandText);

            return cmd;
        }
        public override long ProssimoID(string NomeSequenza)
        {
            IDbConnection conn = new NpgsqlConnection(this.connectionString);

            IDbCommand cmCommand = conn.CreateCommand();
            IDataReader drID;
            string strSql;


            strSql = string.Format("select nextval('{0}')", NomeSequenza);
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

        protected override string SelectIdentity(string SequenceName)
        {
            if (string.IsNullOrEmpty(SequenceName))
            {
                return "SELECT lastval()";
            }
            return string.Format("select currval('{0}')", SequenceName);
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



    }
}



