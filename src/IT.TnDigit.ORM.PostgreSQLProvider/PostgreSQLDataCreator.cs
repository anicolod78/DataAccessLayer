#region Using directives
using IT.TnDigit.ORM.Interfaces;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public partial class PostgreSQLProvider
    {
        public override DataTable GetTables(String owner)
        {
            DataTable result = new DataTable("Tables");

            using (IDbConnection connection = CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT ('{0}.' || table_name) AS table_name FROM information_schema.tables WHERE table_schema='{0}' and table_type='BASE TABLE' order by TABLE_NAME", owner);
                try { connection.Open(); }
                catch
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
                command.CommandText = string.Format("SELECT ('{0}.' || table_name) AS table_name FROM information_schema.tables WHERE table_schema='{0}' and table_type='VIEW' order by TABLE_NAME", owner);

                try { connection.Open(); }
                catch
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
                command.CommandText = string.Format("SELECT DISTINCT '{0}' as PACKAGE_NAME, routine_name as OBJECT_NAME FROM information_schema.routines WHERE routine_schema='{0}'", owner);

                try { connection.Open(); }
                catch
                {
                    return result;
                }


                try { result.Load(command.ExecuteReader()); }
                catch { }

                connection.Close();
            }


            return result;
        }

        protected override string GetProcedureInfo(ref object itm, string owner, ref string packageName, ref string procedureName)
        {
            packageName = "ALTRE";
            if (itm.ToString().Contains("."))
            {
                procedureName = itm.ToString().Split(new char[] { '.' })[1];
            }
            else
            {
                procedureName = itm.ToString();
            }
            return string.Format("select pg_get_function_identity_arguments(p.oid) FROM pg_catalog.pg_proc p where p.proname='{0}';", procedureName);
        }

        protected override IDbDataAdapter GetSpecificDataAdapter(IDbCommand command)
        {
            return new NpgsqlDataAdapter((NpgsqlCommand)command);
        }

        protected override void ClobInfo(StringBuilder sb)
        {
            sb.Append("\t\t\t\t// Use the WriteClob method to insert or update the value of this field\r\n");
        }

        public override string WriteTableObject(object itm, string model, string baseClass, String owner)
        {
            string content = model;

            Hashtable hstPK = null;
            DataSet ds = new DataSet();

            IDbConnection connection = null;
            IDbDataAdapter da = null;
            IDbCommand command = null;


            connection = this.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = string.Format("select * from {0}", GetObjectName(itm));
            da = this.GetSpecificDataAdapter(command);

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.FillSchema(ds, SchemaType.Source);

            DataTable tables = ds.Tables[0];

            hstPK = ReadPrimaryKeys(tables, itm.ToString(), owner);

            content = content.Replace("/*CLASSNAME*/", itm.ToString().Split('.')[1].ClearInvalidCharsU());
            content = content.Replace("/*TABLENAME*/", itm.ToString());
            content = content.Replace("/*BASECLASSNAME*/", baseClass);


            StringBuilder sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                //imposto i tipi nullable tipo? tranne che per le stringhe
                if (dc.DataType.ToString() != "System.Byte[]")
                {
                    if (dc.DataType.ToString() == "System.String" || hstPK.ContainsKey(dc.ColumnName))
                        sb.AppendFormat("\t\tprivate {0} {1};\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsL());
                    else
                        sb.AppendFormat("\t\tprivate {0}? {1};\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsL());
                }
            }
            content = content.Replace("/*FIELDDECLARATION*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in hstPK.Values)
            {

                sb.Append(MappedField.ToString());
                sb.Append(PrimaryKey.ToString());

                sb.AppendFormat("\t\tpublic {0} {1}\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsU());

                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tget\r\n");
                sb.Append("\t\t\t{\r\n");

                sb.AppendFormat("\t\t\t\treturn this.{0};\r\n", dc.ColumnName.ClearInvalidCharsL());

                sb.Append("\t\t\t}\r\n");
                sb.Append("\t\t\tset\r\n");
                sb.Append("\t\t\t{\r\n");
                sb.AppendFormat("\t\t\t\tthis.{0} = value;\r\n", dc.ColumnName.ClearInvalidCharsL());
                sb.Append("\t\t\t}\r\n");
                sb.Append("\t\t}\r\n\r\n");
            }
            content = content.Replace("/*PRIMARYFIELDSPROPERTY*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (hstPK.Contains(dc.ColumnName) == false && dc.DataType.ToString() != "System.Byte[]")
                {
                    sb.Append(MappedField.ToString());
                    if (dc.DataType.ToString() == "System.String")
                    {
                        if (dc.MaxLength > 1)
                        {
                            sb.AppendFormat(MaxLength.ToString(dc.MaxLength));
                        }
                    }

                    if (dc.DataType.ToString() == "System.DateTime")
                    {
                        if (dc.ColumnName.Contains("MODIFICA") || dc.ColumnName.Contains("INSERIMENTO"))
                        {
                            sb.Append(MaxLength.ToString(20));
                        }
                    }

                    if (dc.DataType.ToString() == "System.String" || dc.AllowDBNull == false)
                        sb.AppendFormat("\t\tpublic {0} {1}\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsU());
                    else
                        sb.AppendFormat("\t\tpublic {0}? {1}\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsU());


                    sb.Append("\t\t{\r\n");
                    sb.Append("\t\t\tget\r\n");
                    sb.Append("\t\t\t{\r\n");

                    if (dc.DataType.ToString() != "System.String" && dc.AllowDBNull == false)
                    {
                        sb.AppendFormat("\t\t\t\tif (this.{0}.HasValue)\r\n", dc.ColumnName.ClearInvalidCharsL());
                        sb.AppendFormat("\t\t\t\t\treturn this.{0}.Value;\r\n", dc.ColumnName.ClearInvalidCharsL());
                        sb.AppendFormat("\t\t\t\telse\r\n");
                        switch (dc.DataType.ToString())
                        {
                            case "System.Decimal":
                                sb.AppendFormat("\t\t\t\t\treturn 0;\r\n");
                                break;
                            case "System.Boolean":
                                sb.AppendFormat("\t\t\t\t\treturn false;\r\n");
                                break;
                            default:
                                sb.AppendFormat("\t\t\t\t\treturn default({0});\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType));
                                break;
                        }
                    }
                    else
                    {
                        sb.AppendFormat("\t\t\t\treturn this.{0};\r\n", dc.ColumnName.ClearInvalidCharsL());
                    }


                    sb.Append("\t\t\t}\r\n");
                    sb.Append("\t\t\tset\r\n");
                    sb.Append("\t\t\t{\r\n");

                    if (dc.DataType.ToString() == "System.String"
                        && dc.MaxLength > 10000)
                    {
                        ClobInfo(sb);
                    }

                    if (dc.AllowDBNull)
                    {
                        sb.Append("\t\t\t\t// Set data changed for update operations\r\n");
                        sb.AppendFormat("\t\t\t\tif ({0} != value || value == null)\r\n", dc.ColumnName.ClearInvalidCharsL());
                    }
                    else
                    {
                        sb.Append("\t\t\t\t// Set data changed for update operations\r\n");
                        sb.AppendFormat("\t\t\t\tif ({0} != value)\r\n", dc.ColumnName.ClearInvalidCharsL());
                    }
                    sb.AppendFormat("\t\t\t\t\tthis.SetPropertyChanged(\"{0}\");\r\n", dc.ColumnName.ClearInvalidCharsL());
                    sb.AppendFormat("\t\t\t\tthis.{0} = value;\r\n", dc.ColumnName.ClearInvalidCharsL());
                    sb.Append("\t\t\t}\r\n");
                    sb.Append("\t\t}\r\n\r\n");
                }
            }
            content = content.Replace("/*FIELDSPROPERTY*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                sb.AppendFormat("\t\tpublic static System.String {0}ColumnName\r\n", dc.ColumnName.ClearInvalidCharsU());
                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tget\r\n");
                sb.Append("\t\t\t{\r\n");
                sb.AppendFormat("\t\t\t\treturn \"{0}\";\r\n", dc.ColumnName);
                sb.Append("\t\t\t}\r\n");
                sb.Append("\t\t}\r\n\r\n");
            }

            ////SEQUENCE FOR SERIAL COLUMN
            //if (hstPK.Count > 0)
            //    sb.AppendFormat("\t\tpublic override string SequenceName() { return \"{0}_{1}_SEQ\"; }", itm.ToString(), hstPK[0] );

            content = content.Replace("/*FIELDNAMES*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (dc.DataType.ToString() != "System.Byte[]")
                    sb.AppendFormat("\t\t\tWriteSerialization(writer, this.{0});\r\n", dc.ColumnName.ClearInvalidCharsL());
            }
            content = content.Replace("/*FIELDSSERIALIZE*/", sb.ToString());


            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (dc.DataType.ToString() == "System.String")
                {
                    sb.AppendFormat("\t\t\titem.{0} = ReadSerialization(reader);\r\n", dc.ColumnName.ClearInvalidCharsL());
                }
                else
                {
                    if (dc.DataType.ToString() != "System.Byte[]")
                    {
                        sb.Append("\t\t\ttry {\r\n");
                        sb.AppendFormat("\t\t\t\titem.{0} = {1}.Parse(ReadSerialization(reader));\r\n", dc.ColumnName.ClearInvalidCharsL(),
                            ConvertOracleDataType(dc.ColumnName, dc.DataType));
                        sb.Append("\t\t\t}\r\n\t\t\tcatch {}\r\n");
                    }
                }
            }
            content = content.Replace("/*FIELDSDESERIALIZE*/", sb.ToString());


            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (dc.DataType.ToString() == "System.String")
                {
                    sb.AppendFormat("\t\t\tif (dati[\"{0}\"] is DBNull == false)\r\n", dc.ColumnName.Trim());
                    sb.AppendFormat("\t\t\t\titem.{0} = dati[\"{1}\"].ToString();\r\n", dc.ColumnName.ClearInvalidCharsL(), dc.ColumnName.Trim());
                }
                else
                {
                    if (dc.DataType.ToString() != "System.Byte[]")
                    {
                        sb.AppendFormat("\t\t\tif (dati[\"{0}\"] is DBNull == false)\r\n", dc.ColumnName.Trim());
                        sb.AppendFormat("\t\t\t\titem.{0} = {2}.Parse(dati[\"{1}\"].ToString());\r\n", dc.ColumnName.ClearInvalidCharsL(), dc.ColumnName.Trim(),
                            ConvertOracleDataType(dc.ColumnName, dc.DataType));
                    }
                }
            }
            content = content.Replace("/*FIELDLOADER*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                sb.AppendFormat(",{0}", dc.ColumnName);
            }
            sb.Remove(0, 1);
            content = content.Replace("/*FIELDSLIST*/", sb.ToString());

            return content;
        }

        public override string WriteProcedureObject(Object itm, String model, String baseClass, String paramsDefaultValue, String owner)
        {
            string content = model;

            DataSet ds = new DataSet();
            Hashtable input = new Hashtable();
            Hashtable output = new Hashtable();
            Hashtable inout = new Hashtable();


            IDbConnection connection = null;
            IDbDataAdapter da = null;
            IDbCommand command = null;


            connection = this.GetConnection();

            string packageName = "";
            string procedureName = "";

            command = connection.CreateCommand();
            command.CommandText = GetProcedureInfo(ref itm, owner, ref packageName, ref procedureName);
            if (string.IsNullOrEmpty(command.CommandText))
                return null;

            da = this.GetSpecificDataAdapter(command);

            da.Fill(ds);
            string sArguments = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            String[] argumentsList = sArguments.Split(new char[] { ',' });
            DataTable tables = new DataTable("ARGS");
            tables.Columns.Add("ARGUMENT_NAME");
            tables.Columns.Add("IN_OUT");
            tables.Columns.Add("PLS_TYPE");
            foreach (var a in argumentsList)
            {
                String[] argInfo = a.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                DataRow dr = tables.NewRow();
                if (argInfo.Length == 2)
                {
                    dr["IN_OUT"] = "IN";
                    dr["ARGUMENT_NAME"] = argInfo[0];
                    dr["PLS_TYPE"] = argInfo[1];
                }
                if (argInfo.Length == 3)
                {
                    dr["IN_OUT"] = argInfo[0];
                    dr["ARGUMENT_NAME"] = argInfo[1];
                    dr["PLS_TYPE"] = argInfo[2];
                }
                tables.Rows.Add(dr);
            }

            content = content.Replace("/*PACKAGE*/", packageName);
            content = content.Replace("/*CLASSNAME*/", procedureName);
            content = content.Replace("/*CLASSNAMECOMPLETO*/", itm.ToString());
            content = content.Replace("/*PROCEDURENAME*/", itm.ToString());

            content = content.Replace("/*BASECLASSNAME*/", baseClass);

            foreach (DataRow dr in tables.Rows)
            {
                if (string.IsNullOrEmpty(dr["ARGUMENT_NAME"].ToString()))
                    continue;

                switch (dr["IN_OUT"].ToString())
                {
                    case "IN":
                        input.Add(dr["ARGUMENT_NAME"].ToString(), dr["PLS_TYPE"].ToString());
                        break;
                    case "OUT":
                        output.Add(dr["ARGUMENT_NAME"].ToString(), dr["PLS_TYPE"].ToString());
                        break;
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (string k in input.Keys)
            {
                sb.AppendFormat("\t\t\tthis.input.Add(\"{0}\", new FrameworkStoredProcedureParameter(\"{0}\", ParameterDirection.Input, {1}, {2}));\r\n",
                    k, TipoParametro(input[k].ToString()), paramsDefaultValue);
            }
            content = content.Replace("/*PARAMETRIINPUT*/", sb.ToString());

            sb = new StringBuilder();
            foreach (string k in output.Keys)
            {

                sb.AppendFormat("\t\t\tthis.output.Add(\"{0}\", new FrameworkStoredProcedureParameter(\"{0}\", ParameterDirection.Output, {1}, {2}));\r\n",
                    k, TipoParametro(output[k].ToString()), paramsDefaultValue);
            }
            content = content.Replace("/*PARAMETRIOUTPUT*/", sb.ToString());

            sb = new StringBuilder();
            foreach (string k in input.Keys)
            {
                sb.AppendFormat("\t\tpublic {1} {0}\r\n", k, TipoValoreParametro(input[k].ToString()));
                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tset {\r\n");
                sb.AppendFormat("\t\t\t\tthis.input[\"{0}\"].Valore=value ;\r\n", k);
                sb.Append("\t\t\t}\r\n\t\t}\r\n\r\n");
            }
            content = content.Replace("/*PARAMETRIINPUTSET*/", sb.ToString());

            sb = new StringBuilder();
            foreach (string k in output.Keys)
            {
                sb.AppendFormat("\t\tpublic string OUT_{0}\r\n", k);
                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tget {\r\n");
                sb.AppendFormat("\t\t\t\treturn \"{0}\" ;\r\n", k);
                sb.Append("\t\t\t}\r\n\t\t}\r\n\r\n");

                sb.AppendFormat("\t\tpublic {1} {0}\r\n", k, TipoValoreParametro(output[k].ToString()));
                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tget {\r\n");
                sb.AppendFormat("\t\t\t\treturn ({1})this.output[\"{0}\"].Valore;\r\n", k, TipoValoreParametro(output[k].ToString()));
                sb.Append("\t\t\t}\r\n\t\t}\r\n\r\n");
            }
            content = content.Replace("/*PARAMETRIOUTPUTNAMES*/", sb.ToString());

            return content;
        }

        protected override Hashtable ReadPrimaryKeys(DataTable tables, String item, string owner)
        {
            Hashtable hstPK = new Hashtable();

            IDbCommand cmd2 = connection.CreateCommand();

            cmd2.CommandText = string.Format(
                "SELECT a.attname as COLUMN_NAME, '{0}' as TABLE_NAME FROM pg_index i JOIN pg_attribute a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey) WHERE i.indrelid = '{0}'::regclass AND i.indisprimary",
                item);

            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter((NpgsqlCommand)cmd2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "CONTRAINTS");

            DataTable pkdt = ds2.Tables["CONTRAINTS"];
            foreach (DataRow dr in pkdt.Rows)
            {
                hstPK.Add(dr["COLUMN_NAME"].ToString(), tables.Columns[dr["COLUMN_NAME"].ToString()]);
            }

            return hstPK;
        }

        public override Dictionary<string, object> DatabaseParams()
        {
            var result = new Dictionary<string, object>();

            result.Add("Server_Enabled", true);
            result.Add("ChooseDB_Enabled", false);
            result.Add("Database_Enabled", true);
            result.Add("Owner_Enabled", true);

            return result;
        }
    }
}



