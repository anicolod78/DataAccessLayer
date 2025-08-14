#region Using directives

using IT.TnDigit.ORM.Interfaces;
using System;
using System.Data;
using System.Reflection;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public partial class SQLClientProvider : DataProvider
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
            return new System.Data.SqlClient.SqlConnection(this.connectionString);
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

        public override string parameterPlaceHolder
        {
            get
            {
                return "@P";
            }
        }

        public override string DateFilter(string data)
        {
            return string.Format("CAST('{0}' as datetime)", data);
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
            string cmdText;

            if (risultati > 0)
            {
                cmdText = "Select * from (select {0}.*{3},row_number() over ({2}) as num From {0}{1}) as a where num>{4} and num<={5}";

                //ordinamento obbligatorio
                if (ordinamento != "" && ordinamento != null)
                    ordinamento = " Order by " + ordinamento;
                else
                    throw new ArgumentNullException("ORDINAMENTO");

                string f = filtro.ToString();
                if (f != null && f != "")
                {
                    f = " Where " + f;
                }

                if (this.GetAggiunteSelect(prototype) != null)
                    cmd.CommandText = string.Format(cmdText,
                        this.GetTableName(prototype),
                        f,
                        ordinamento,
                        "," + this.GetAggiunteSelect(prototype),
                        ((pagina - 1) * elementiPerPagina),
                        (pagina * elementiPerPagina));
                else
                    cmd.CommandText = string.Format(cmdText,
                        this.GetTableName(prototype),
                        f,
                        ordinamento,
                        "",
                        ((pagina - 1) * elementiPerPagina),
                        (pagina * elementiPerPagina));
            }
            else
            {
                return null;
            }
            ActivateBindByName(cmd);
            FillQueryParameter(filtro, cmd);
            return cmd;
        }

        protected override IDbCommand GetItemsCommand(IDbConnection conn, SupportCreaElemento prototype, IFilter filtro, string ordinamento, string field, int elementiPerPagina, int pagina, ref int risultati)
        {
            string cmdText = "Select Count({0}) From (Select DISTINCT {0} from {1}{2})";

            IDbCommand cmd = conn.CreateCommand();
            string f = filtro.ToString();
            if (f != null && f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }
            ActivateBindByName(cmd);
            FillQueryParameter(filtro, cmd);

            cmd.CommandText = string.Format(cmdText, field, this.GetTableName(prototype), f);

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.CommandTimeout = 180;
            int contatore = int.Parse(cmd.ExecuteScalar().ToString());
            risultati = contatore;

            //conn.Close();

            if (contatore > 0)
            {
                cmdText = "Select * from (select TB.{3},row_number() over ({2}) as num  from (SELECT DISTINCT {0}.{3} from {0}{1})as TB )  as a where num>{4} and num<={5}";

                //ordinamento obbligatorio
                if (ordinamento != "" && ordinamento != null)
                    ordinamento = " Order by " + ordinamento;
                else
                    return null;

                cmd.CommandText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, field, ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));
            }
            else
            {
                return null;
            }

            return cmd;
        }

        public override string GetSQLCommand(SupportCreaElemento prototype, IFilter filtro, string ordinamento, int elementiPerPagina, int pagina)
        {
            string cmdText = "Select * from (select {0}.*{3},row_number() over ({2} as num From {0}{1}) as a where num>{4} and num<={5}";

            string f = filtro.ToNonParametricString();
            if (f != null && f != "")
            {
                f = " Where " + f;
                f = f.Replace("*", JollyCharacter);
            }

            //ordinamento obbligatorio
            if (ordinamento != "" && ordinamento != null)
                ordinamento = " Order by " + ordinamento;
            else
                return null;

            if (this.GetAggiunteSelect(prototype) != null)
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "," + this.GetAggiunteSelect(prototype), ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));
            else
                cmdText = string.Format(cmdText, this.GetTableName(prototype), f, ordinamento, "", ((pagina - 1) * elementiPerPagina), (pagina * elementiPerPagina));

            return cmdText;
        }

        #endregion

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
            int bufferSize = 100;
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

    }
}



