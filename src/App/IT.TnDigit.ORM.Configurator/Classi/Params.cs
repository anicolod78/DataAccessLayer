using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace IT.TnDigit.ORM.Configurator.Classi
{
    public class Params
    {
        static Params instance;

        public Connection Connection = new Connection();
        public Domain Domain = new Domain();
        public Selections Objects = new Selections();

        private Params()
        {
            Objects.Tables = new Dictionary<string, Selections.Table>();
            Objects.Views = new List<string>();
            Objects.Functions = new List<string>();
        }

        public static Params Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Params();
                }


                return instance;
            }
        }
    }

    public enum FrameworkDBType
    {
        None = 0,
        [Description("")]
        OracleClient = 1,
        [Description("")]
        Access = 2,
        [Description("")]
        SQLServer = 3,
        [Description("")]
        SQLServerFile = 4,
        [Description("")]
        SQLite = 5,
        ODP10 = 6,
        SqlServerCE = 7,
        PostgreSQL = 8,
        Odbc = 9,
        [Description("")]
        Oracle = 10,
    }

    public struct Connection
    {
        public string dataProvider { get; set; }

        private String tipoDatabase;

        public String TipoDatabase
        {
            get { return tipoDatabase; }
            set { tipoDatabase = value; }
        }

        private string server;
        private string database;
        private string owner;
        private string utente;
        private string password;
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }


        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        public string Utente
        {
            get { return utente; }
            set { utente = value; }
        }


        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }


        public string Database
        {
            get { return database; }
            set { database = value; }
        }


        public string Server
        {
            get { return server; }
            set { server = value; }
        }
    }

    public class Domain
    {
        private string nameSpace;
        private string baseClassTables = "FrameworkObject";
        private string baseClassViews = "FrameworkObject";
        private string baseClassProcs = "FrameworkStoredProcedure";
        private string paramsDefaultValue = "null";
        private string filePath;
        private bool numericiSpecifici;

        public bool NumericiSpecifici
        {
            get { return numericiSpecifici; }
            set { numericiSpecifici = value; }
        }


        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }


        public string ParamsDefaultValue
        {
            get { return paramsDefaultValue; }
            set { paramsDefaultValue = value; }
        }


        public string BaseClassProcs
        {
            get { return baseClassProcs; }
            set { baseClassProcs = value; }
        }


        public string BaseClassViews
        {
            get { return baseClassViews; }
            set { baseClassViews = value; }
        }


        public string BaseClassTables
        {
            get { return baseClassTables; }
            set { baseClassTables = value; }
        }


        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }

        public string Progetto { get; set; }
    }

    [Serializable]
    public struct Selections
    {
        public string Serialize()
        {
            return "not implemented";
            /*using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, this);
                return Convert.ToBase64String(stream.ToArray());
            }*/
        }

        public static Selections Deserialize(string value)
        {
            return new Selections();
            /*byte[] bytes = Convert.FromBase64String(value);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter b = new BinaryFormatter();
                stream.Position = 0;
                var result = (Selections)b.Deserialize(stream);
                return result;
            }*/
        }

        public string SerializeXML()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Selections));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, this);
            string result = sww.ToString();
            sww.Close();
            return HttpUtility.HtmlEncode(result);
        }

        public static Selections DeserializeXML(string value)
        {
            value = HttpUtility.HtmlDecode(value);
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Selections));
            StringReader sww = new StringReader(value);
            var obj = xsSubmit.Deserialize(sww);
            sww.Close();
            return (Selections)obj;
        }

        public Dictionary<String, Table> Tables { get; set; }

        public List<String> Views { get; set; }

        public List<String> Functions { get; set; }

        [Serializable]
        public class Table
        {
            public string Name { get; set; }

            public bool AdvancedConfig { get; set; }

            public List<Field> Campi = new List<Field>();

            public override string ToString()
            {
                return this.Name + (this.AdvancedConfig ? " (+)" : "");
            }
        }

        [Serializable]
        public class Field
        {
            public String Name { get; set; }

            public List<Attribute> Attributes = new List<Attribute>();
        }
    }
}
