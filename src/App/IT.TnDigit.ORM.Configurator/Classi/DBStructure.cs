using IT.TnDigit.ORM.DataProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace IT.TnDigit.ORM.Configurator.Classi
{
    static class DBStructure
    {
        public static void GenerateConnectionString()
        {
            switch (Params.Instance.Connection.TipoDatabase)
            {              
                case "PostgreSQLProvider":
                    Params.Instance.Connection.ConnectionString =
                        string.Format("Server={0};Port=5432;Database={1};User Id={2};Password={3};",
                        Params.Instance.Connection.Server,
                        Params.Instance.Connection.Database,
                        Params.Instance.Connection.Utente,
                        Params.Instance.Connection.Password);
                    break;
                default:
                    break;
            }
        }


        public static DataProvider ProviderFactory()
        {
            DataProvider p = null;
           
            try
            {
                var type = Params.Instance.Connection.TipoDatabase.Split('|');
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string assemblyPath = Path.Combine(appDirectory, type[0]);

                // Load the assembly from the application folder
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                // Create instance using full type name
                p = (DataProvider)Activator.CreateInstance(assembly.GetType(type[1]));
            }
            catch
            {
                MessageBox.Show("Impossibile istanziare il provider di tipo " + Params.Instance.Connection.TipoDatabase);
                return p;
            }
            if (!string.IsNullOrEmpty(Params.Instance.Connection.ConnectionString))
                p.Initialize(Params.Instance.Connection.ConnectionString);
            return p;
        }

        public static DataTable GetTables()
        {
            var p = ProviderFactory();
            return p.GetTables(Params.Instance.Connection.Owner);
        }

        public static DataTable GetViews()
        {
            var p = ProviderFactory();
            return p.GetViews(Params.Instance.Connection.Owner);
        }

        public static DataTable GetProcedures()
        {
            var p = ProviderFactory();
            return p.GetProcedures(Params.Instance.Connection.Owner);
        }

        public static Exception CheckConnection()
        {
            var p = ProviderFactory();
            if (p == null)
                return new Exception("Database non configurato");

            return p.CheckConnection();
        }

        public static Dictionary<string, object> DatabaseParams()
        {
            var p = ProviderFactory();
            if (p == null)
                return null;

            return p.DatabaseParams();
        }
    }
}
