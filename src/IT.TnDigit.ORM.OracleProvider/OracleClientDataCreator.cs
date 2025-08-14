#region Using directives

using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public partial class OracleClientProvider
    {
        public override DataTable GetTables(String owner)
        {
            DataTable result = new DataTable("Tables");

            using (IDbConnection connection = CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = string.Format("select TABLE_NAME from all_TABLES where owner = '{0}' AND secondary = 'N' order by TABLE_NAME", owner);

                try { connection.Open(); }
                catch (Exception ex)
                {
                    return result;
                }


                try { result.Load(command.ExecuteReader()); }
                catch { }

                connection.Close();
            }

            return result;
        }

        public override DataTable GetViews(string owner)
        {
            DataTable result = new DataTable("Views");

            using (IDbConnection connection = CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = string.Format("select VIEW_NAME as table_Name from all_VIEWS where owner = '{0}' order by VIEW_NAME", owner);

                try { connection.Open(); }
                catch (Exception ex)
                {
                    return result;
                }


                try { result.Load(command.ExecuteReader()); }
                catch { }

                connection.Close();
            }

            return result;
        }

        public override DataTable GetProcedures(string owner)
        {
            DataTable result = new DataTable("Procedures");

            using (IDbConnection connection = CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT DISTINCT PACKAGE_NAME, OBJECT_NAME  from all_arguments where owner = '{0}' and DATA_TYPE <> 'REF CURSOR' order by PACKAGE_NAME, OBJECT_NAME", owner);

                try { connection.Open(); }
                catch (Exception ex)
                {
                    return result;
                }


                try { result.Load(command.ExecuteReader()); }
                catch { }

                connection.Close();
            }


            return result;
        }

        protected override IDbDataAdapter GetSpecificDataAdapter(IDbCommand command)
        {
            return new OracleDataAdapter((OracleCommand)command);
        }

        protected override string GetProcedureInfo(ref object itm, string owner, ref string packageName, ref string procedureName)
        {
            if (itm.ToString().Contains("."))
            {
                packageName = itm.ToString().Split(Convert.ToChar("."))[0];
                procedureName = itm.ToString().Split(Convert.ToChar("."))[1];
                return string.Format("select ARGUMENT_NAME, IN_OUT, PLS_TYPE from all_arguments where owner = '{0}' and DATA_TYPE <> 'REF CURSOR' and PACKAGE_NAME='{1}' and OBJECT_NAME='{2}'",
                    owner, packageName, procedureName);
            }
            else
            {
                packageName = "ALTRE";
                procedureName = itm.ToString();
                return string.Format("select ARGUMENT_NAME, IN_OUT, PLS_TYPE from all_arguments where owner = '{0}' and DATA_TYPE <> 'REF CURSOR' and PACKAGE_NAME IS NULL and OBJECT_NAME='{1}'",
                    owner, procedureName);
            }
        }

        protected override Hashtable ReadPrimaryKeys(DataTable tables, String item, string owner)
        {
            Hashtable hstPK = new Hashtable();

            IDbCommand cmd2 = connection.CreateCommand();

            cmd2.CommandText = string.Format(
                "select ALL_CONS_COLUMNS.COLUMN_NAME,ALL_CONSTRAINTS.TABLE_NAME from ALL_CONS_COLUMNS,ALL_CONSTRAINTS WHERE ALL_CONS_COLUMNS.CONSTRAINT_NAME = ALL_CONSTRAINTS.CONSTRAINT_NAME AND ALL_CONSTRAINTS.CONSTRAINT_TYPE ='P' AND UPPER(ALL_CONSTRAINTS.TABLE_NAME)='{0}' AND ALL_CONSTRAINTS.OWNER='{1}'",
                item, owner);

            OracleDataAdapter da2 = new OracleDataAdapter((OracleCommand)cmd2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "CONTRAINTS");

            DataTable pkdt = ds2.Tables["CONTRAINTS"];
            foreach (DataRow dr in pkdt.Rows)
            {
                hstPK.Add(dr["COLUMN_NAME"].ToString(), tables.Columns[dr["COLUMN_NAME"].ToString()]);
            }

            return hstPK;
        }

        protected override void ClobInfo(StringBuilder sb)
        {
            sb.Append("\t\t\t\t// Use the WriteClob method to insert or update the value of this field\r\n");
        }

        public override Dictionary<string, object> DatabaseParams()
        {
            var result = new Dictionary<string, object>();

            result.Add("Server_Enabled", false);
            result.Add("ChooseDB_Enabled", false);
            result.Add("Database_Enabled", true);
            result.Add("Owner_Enabled", true);

            return result;
        }
    }
}



