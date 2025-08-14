#region Using directives

using IT.TnDigit.ORM.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#endregion

namespace IT.TnDigit.ORM.DataProviders
{
    public abstract partial class DataProvider : IDataTypesCreatorProvider
    {
        public virtual DataTable GetTables(String owner)
        {
            DataTable result = new DataTable("Tables");
            return result;
        }

        public virtual DataTable GetViews(String owner)
        {
            DataTable result = new DataTable("Views");
            return result;
        }

        public virtual DataTable GetProcedures(String owner)
        {
            DataTable result = new DataTable("Procedures");
            return result;
        }

        protected virtual IDbDataAdapter GetSpecificDataAdapter(IDbCommand command)
        {
            return null;
        }

        protected virtual string GetObjectName(Object itm)
        {
            return itm.ToString();
        }

        public virtual DataColumnCollection GetColumnsList(Object itm)
        {
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

            return ds.Tables[0].Columns;
        }

        public virtual string WriteTableObject(Object itm, String model, String baseClass, String owner)
        {
            string content = model;

            Hashtable hstPK = null;
            DataSet ds = new DataSet();

            IDbConnection connection = null;
            IDbDataAdapter da = null;
            IDbCommand command = null;

            content = content.Replace("/*DPNAME*/", this.GetType().Name);
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            content = content.Replace("/*DPVERSION*/", fvi.FileVersion);
            assembly = Assembly.GetCallingAssembly();
            fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            content = content.Replace("/*DANVERSION*/", fvi.FileVersion);

            connection = this.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = string.Format("select * from {0}", GetObjectName(itm));
            da = this.GetSpecificDataAdapter(command);

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.FillSchema(ds, SchemaType.Source);

            DataTable tables = ds.Tables[0];

            hstPK = this.ReadPrimaryKeys(tables, itm.ToString(), owner);

            if (itm.ToString().Contains("."))
                content = content.Replace("/*CLASSNAME*/", itm.ToString().Replace('.', '_').ClearInvalidCharsU());
            else
                content = content.Replace("/*CLASSNAME*/", itm.ToString().ClearInvalidCharsU());

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
                        if (dc.ColumnName.ToUpper().Contains("MODIFICA")
                            || dc.ColumnName.ToUpper().Contains("INSERIMENTO")
                            || dc.ColumnName.ToUpper().Contains("CREATE")
                            || dc.ColumnName.ToUpper().Contains("UPDATE")
                            || dc.ColumnName.ToUpper().Equals("CDATE")
                            || dc.ColumnName.ToUpper().Equals("MDATE")
                            || dc.ColumnName.ToUpper().Equals("TTIMEVAR"))
                        {
                            sb.Append(AllowDateTime.ToString(true));
                        }
                    }

                    if (dc.DataType.ToString() == "System.String" && dc.MaxLength > 10000)
                    {
                        sb.Append("\t\t[Obsolete(\"Use the WriteClob method to insert or update the value of this field\")]\r\n");
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
                                sb.AppendFormat("\t\t\t\t\treturn {0}.MinValue;\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType));
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

                    if (dc.DataType.ToString() == "System.String" && dc.MaxLength > 10000)
                    {
                        ClobInfo(sb);
                    }
                    else
                    {
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
                    }

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
                sb.AppendFormat(",{0}", dc.ColumnName.ToUpper());
            }
            sb.Remove(0, 1);
            content = content.Replace("/*FIELDSLIST*/", sb.ToString());

            return content;
        }

        public virtual string WriteViewObject(Object itm, String model, String baseClass)
        {
            string content = model;

            DataSet ds = new DataSet();

            IDbConnection connection = null;
            IDbDataAdapter da = null;
            IDbCommand command = null;

            content = content.Replace("/*DPNAME*/", this.GetType().Name);
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            content = content.Replace("/*DPVERSION*/", fvi.FileVersion);
            assembly = Assembly.GetCallingAssembly();
            fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            content = content.Replace("/*DANVERSION*/", fvi.FileVersion);

            connection = this.GetConnection();
            command = connection.CreateCommand();
            command.CommandText = string.Format("select * from {0}", GetObjectName(itm));
            da = this.GetSpecificDataAdapter(command);

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.FillSchema(ds, SchemaType.Source);

            DataTable tables = ds.Tables[0];

            if (itm.ToString().Contains("."))
                content = content.Replace("/*CLASSNAME*/", itm.ToString().Replace('.', '_').ClearInvalidCharsU());
            else
                content = content.Replace("/*CLASSNAME*/", itm.ToString().ClearInvalidCharsU());

            content = content.Replace("/*TABLENAME*/", itm.ToString());
            content = content.Replace("/*BASECLASSNAME*/", baseClass);

            StringBuilder sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (dc.DataType.ToString() != "System.Byte[]")
                {
                    if (dc.DataType.ToString() == "System.String" || dc.AllowDBNull == false)
                        sb.AppendFormat("\t\tprivate {0} {1};\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsL());
                    else
                        sb.AppendFormat("\t\tprivate {0}? {1};\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsL());
                }
            }
            content = content.Replace("/*FIELDDECLARATION*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (dc.DataType.ToString() != "System.Byte[]")
                {
                    if (dc.DataType.ToString() == "System.String" || dc.AllowDBNull == false)
                        sb.AppendFormat("\t\tpublic {0} {1}\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsU());
                    else
                        sb.AppendFormat("\t\tpublic {0}? {1}\r\n", ConvertOracleDataType(dc.ColumnName, dc.DataType), dc.ColumnName.ClearInvalidCharsU());

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
                if (dc.DataType.Name.ToUpper() == "STRING")
                {
                    sb.AppendFormat("\t\t\titem.{0} = ReadSerialization(reader);\r\n", dc.ColumnName.ClearInvalidCharsL());
                }
                else
                {
                    sb.Append("\t\t\ttry {\r\n");
                    if (dc.DataType.ToString() != "System.Byte[]")
                        sb.AppendFormat("\t\t\titem.{0} = {1}.Parse(ReadSerialization(reader));\r\n", dc.ColumnName.ClearInvalidCharsL(),
                            ConvertOracleDataType(dc.ColumnName, dc.DataType));
                    sb.Append("\t\t\t}\r\n\t\t\tcatch {}\r\n");
                }
            }
            content = content.Replace("/*FIELDSDESERIALIZE*/", sb.ToString());

            sb = new StringBuilder();
            foreach (DataColumn dc in tables.Columns)
            {
                if (dc.DataType.Name.ToUpper() == "STRING")
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
                sb.AppendFormat(",{0}", dc.ColumnName.ToUpper());
            }
            sb.Remove(0, 1);
            content = content.Replace("/*FIELDSLIST*/", sb.ToString());

            return content;
        }

        public virtual string WriteProcedureObject(Object itm, String model, String baseClass, String paramsDefaultValue, String owner)
        {
            string content = model;

            DataSet ds = new DataSet();
            Hashtable input = new Hashtable();
            Hashtable output = new Hashtable();
            Hashtable inout = new Hashtable();
            Hashtable result = new Hashtable();

            IDbConnection connection = null;
            IDbDataAdapter da = null;
            IDbCommand command = null;

            content = content.Replace("/*DPNAME*/", this.GetType().Name);
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            content = content.Replace("/*DPVERSION*/", fvi.FileVersion);
            assembly = Assembly.GetCallingAssembly();
            fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            content = content.Replace("/*DANVERSION*/", fvi.FileVersion);

            connection = this.GetConnection();

            string packageName = "";
            string procedureName = "";

            command = connection.CreateCommand();

            if (this.GetType().Name.EndsWith("SqlClientProvider"))
            {
                if (itm.ToString().Split(Convert.ToChar("."))[0] == "TF")
                    content = content.Replace("/*RETURNTABLE*/", "this.ReturnTable = true;");
            }

            command.CommandText = GetProcedureInfo(ref itm, owner, ref packageName, ref procedureName);
            if (string.IsNullOrEmpty(command.CommandText))
                return null;

            da = this.GetSpecificDataAdapter(command);

            da.Fill(ds);
            DataTable tables = ds.Tables[0];

            content = content.Replace("/*PACKAGE*/", packageName);
            content = content.Replace("/*CLASSNAME*/", procedureName);
            content = content.Replace("/*CLASSNAMECOMPLETO*/", itm.ToString());
            content = content.Replace("/*PROCEDURENAME*/", itm.ToString());

            content = content.Replace("/*BASECLASSNAME*/", baseClass);

            foreach (DataRow dr in tables.Rows)
            {
                string argname = dr["ARGUMENT_NAME"].ToString();
                string direction = dr["IN_OUT"].ToString();
                if (string.IsNullOrEmpty(argname))
                {
                    if (dr["IN_OUT"].ToString() == "OUT" && !output.ContainsKey("RETURN"))  //return value
                    {
                        argname = "RETURN";
                        direction = "RESULT";
                    }
                    else
                        continue;
                }

                switch (direction)
                {
                    case "IN":
                        input.Add(argname, dr["PLS_TYPE"].ToString());
                        break;
                    case "OUT":
                        output.Add(argname, dr["PLS_TYPE"].ToString());
                        break;
                    case "RESULT":
                        result.Add(argname, dr["PLS_TYPE"].ToString());
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
            foreach (string k in result.Keys)
            {
                sb.AppendFormat("\t\t\tthis.output.Add(\"{0}\", new FrameworkStoredProcedureParameter(\"{0}\", ParameterDirection.ReturnValue, {1}, {2}));\r\n",
                    k, TipoParametro(result[k].ToString()), paramsDefaultValue);
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
            foreach (string k in result.Keys)
            {
                sb.AppendFormat("\t\tpublic string OUT_{0}\r\n", k);
                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tget {\r\n");
                sb.AppendFormat("\t\t\t\treturn \"{0}\" ;\r\n", k);
                sb.Append("\t\t\t}\r\n\t\t}\r\n\r\n");

                sb.AppendFormat("\t\tpublic {1} {0}\r\n", k, TipoValoreParametro(result[k].ToString()));
                sb.Append("\t\t{\r\n");
                sb.Append("\t\t\tget {\r\n");
                sb.AppendFormat("\t\t\t\treturn ({1})this.output[\"{0}\"].Valore;\r\n", k, TipoValoreParametro(result[k].ToString()));
                sb.Append("\t\t\t}\r\n\t\t}\r\n\r\n");
            }
            content = content.Replace("/*PARAMETRIOUTPUTNAMES*/", sb.ToString());

            return content;
        }

        protected virtual string GetProcedureInfo(ref object itm, string owner, ref string packageName, ref string procedureName)
        {
            return null;
        }

        protected static string TipoParametro(string Tipo)
        {
            switch (Tipo.ToUpper())
            {
                case "NUMBER":
                case "DECIMAL":
                case "NUMERIC":
                case "MONEY":
                case "SMALLMONEY":
                    return "DbType.Decimal";
                case "BIT":
                    return "DbType.Boolean";
                case "SMALLINT":
                    return "DbType.Int16";
                case "TINYINT":
                    return "DbType.Byte";
                case "INTEGER":
                case "INT":
                    return "DbType.Int32";
                case "LONG":
                case "BIGINT":
                    return "DbType.Int64";
                case "VARCHAR2":
                case "VARCHAR":
                case "TEXT":
                    return "DbType.String";
                case "DATE":
                case "DATETIME":
                case "SMALLDATETIME":
                    return "DbType.DateTime";
                case "REAL":
                    return "DbType.Single";
                case "FLOAT":
                    return "DbType.Double";
                case "BLOB":
                case "VARBINARY":
                case "BINARY":
                    return "DbType.Binary";
                case "CLOB":
                    return "DbType.AnsiString";
                default:
                    return "DbType.Object";
            }
        }

        protected static string TipoValoreParametro(string Tipo)
        {
            switch (Tipo.ToUpper())
            {
                case "NUMBER":
                case "DECIMAL":
                case "NUMERIC":
                case "MONEY":
                case "SMALLMONEY":
                    return "System.Decimal?";
                case "BIT":
                    return "System.Boolean?";
                case "SMALLINT":
                    return "System.Int16?";
                case "TINYINT":
                    return "System.Byte?";
                case "INTEGER":
                case "INT":
                    return "System.Int32?";
                case "LONG":
                case "BIGINT":
                    return "System.Int64?";
                case "VARCHAR2":
                case "VARCHAR":
                case "CLOB":
                    return "System.String";
                case "DATE":
                case "DATETIME":
                case "SMALLDATETIME":
                    return "System.DateTime?";
                case "REAL":
                    return "System.Single?";
                case "FLOAT":
                    return "System.Double?";
                case "BLOB":
                case "VARBINARY":
                case "BINARY":
                    return "System.Byte[]";
                default:
                    return "System.Object";
            }
        }

        protected virtual string ConvertOracleDataType(string columnName, Type dataType)
        {
            return dataType.ToString();
        }

        protected virtual void ClobInfo(StringBuilder sb)
        {
            sb.Append("\t\t\t\t// Use the WriteClob method to insert or update the value of this field\r\n");
        }

        protected virtual Hashtable ReadPrimaryKeys(DataTable tables, String item, string owner)
        {
            var hstPK = new Hashtable();

            foreach (DataColumn dc in tables.PrimaryKey)
            {
                hstPK.Add(dc.ColumnName, dc);
            }

            return hstPK;
        }

        public virtual Exception CheckConnection()
        {
            IDbConnection connection = null;
            try
            {
                connection = this.GetConnection();
            }
            catch (Exception ex)
            {
                return ex;
            }

            try
            {
                connection.Open();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
            finally
            {
                connection.Close();
            }

        }

        public virtual Dictionary<string, object> DatabaseParams()
        {
            var result = new Dictionary<string, object>();

            result.Add("Server_Enabled", false);
            result.Add("ChooseDB_Enabled", false);
            result.Add("Database_Enabled", false);
            result.Add("Owner_Enabled", false);

            return result;
        }
    }

    public static class StringExtensions
    {
        private static string Clear(string value)
        {
            return value.Trim().Replace(" ", "").Replace("-", "_").Replace("%", "perc").Replace("/", "_");
        }

        public static string ClearInvalidCharsU(this string value)
        {
            return Clear(value).ToUpper();
        }

        public static string ClearInvalidCharsL(this string value)
        {
            return Clear(value).ToLower();
        }
    }
}

