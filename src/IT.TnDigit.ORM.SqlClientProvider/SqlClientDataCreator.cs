#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public partial class SQLClientProvider
    {
        public override DataTable GetTables(string owner)
        {
            DataTable result = new DataTable("Tables");

            using (IDbConnection connection = CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = "select (TABLE_SCHEMA + '.' + table_name) as table_name from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE' order by TABLE_SCHEMA, table_name";

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
                command.CommandText = "select (TABLE_SCHEMA + '.' + table_name) as table_name from INFORMATION_SCHEMA.Views order by TABLE_SCHEMA, table_name";

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
                command.CommandText = "Select (type + '.' + STR(id)) as PACKAGE_NAME, name as OBJECT_NAME  from sysobjects where (type = 'P' or type = 'FN'  or type = 'TF') and category = 0 order by name";

                try { connection.Open(); }
                catch (Exception ex)
                {
                    return result;
                }

                try { result.Load(command.ExecuteReader()); }
                catch (Exception ex)
                {
                    var s = ex.Message;
                }

                connection.Close();
            }

            return result;
        }

        protected override string GetProcedureInfo(ref object itm, string owner, ref string packageName, ref string procedureName)
        {
            if (itm.ToString().Contains("."))
            {
                var s = itm.ToString().Split(Convert.ToChar("."));
                packageName = s[1];
                procedureName = s[2];
                string sql = string.Format("SELECT syscolumns.name as ARGUMENT_NAME, (CASE WHEN isoutparam = 0 THEN 'IN' ELSE 'OUT' END) as IN_OUT, systypes.Name as PLS_TYPE FROM syscolumns, systypes WHERE syscolumns.xtype = systypes.xtype AND syscolumns.id = {0}", packageName);
                packageName = "ALTRE";
                itm = procedureName;
                return sql;
            }
            else
            {
                return null;
            }
        }

        protected override IDbDataAdapter GetSpecificDataAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        public override Dictionary<string, object> DatabaseParams()
        {
            var result = new Dictionary<string, object>();

            result.Add("Server_Enabled", true);
            result.Add("ChooseDB_Enabled", true);
            result.Add("Database_Enabled", false);
            result.Add("Owner_Enabled", false);

            return result;
        }
    }
}



