using IT.TnDigit.ORM.Configurator.Classi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace IT.TnDigit.ORM.Configurator.Configurations
{
    public partial class ucDBConfig : UserControl, IucBase
    {
        public event EventHandler SaveConfig;
        public event EventHandler Cancel;

        public ucDBConfig()
        {
            InitializeComponent();
        }

        private void ucDBConfig_Load(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            List<string> assemblies = new List<string>();

            assemblies.Add("---");
            foreach (string dll in Directory.GetFiles(path, "IT.TnDigit.ORM.DataProviders.*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFile(dll);
                    var dataProviderAssemby = loadedAssembly.GetTypes().Where(a => a.BaseType == typeof(DataProviders.DataProvider)).FirstOrDefault();
  
                    if (dataProviderAssemby != null)
                        assemblies.Add($"{loadedAssembly.ManifestModule}|{dataProviderAssemby.FullName}");
                }
                catch (FileLoadException loadEx)
                {
                } // The Assembly has already been loaded.
                catch (BadImageFormatException imgEx)
                {
                } // If a BadImageFormatException exception is thrown, the file is not an assembly.
            }

            cmbTipoDB.DataSource = assemblies; // Enum.GetValues(typeof(FrameworkDBType));

            cmbTipoDB.SelectedItem = Params.Instance.Connection.TipoDatabase;
            if (cmbTipoDB.SelectedItem == null && assemblies.Count > 0)
                cmbTipoDB.SelectedIndex = 0;
        }

        private void cmbTipoDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoDB.SelectedIndex <= 0)
            {
                pnlDatabaseParams.Enabled = false;
                return;
            }

            Params.Instance.Connection.TipoDatabase = cmbTipoDB.SelectedItem.ToString();

            txtServer.Text = string.Empty;

            var DBpar = DBStructure.DatabaseParams();
            if (DBpar != null)
            {
                txtServer.Enabled = (bool)DBpar["Server_Enabled"];
                btnChooseDB.Enabled = (bool)DBpar["ChooseDB_Enabled"];
                txtDatabase.ReadOnly = !(bool)DBpar["Database_Enabled"];
                txtOwner.Enabled = (bool)DBpar["Owner_Enabled"];

                pnlDatabaseParams.Enabled = true;
            }

            txtDatabase.Text = Params.Instance.Connection.Database;
            txtOwner.Text = Params.Instance.Connection.Owner;
            txtPassword.Text = Params.Instance.Connection.Password;
            txtServer.Text = Params.Instance.Connection.Server;
            txtUtente.Text = Params.Instance.Connection.Utente;
        }

        private void btnChooseDB_Click(object sender, EventArgs e)
        {
            switch (Params.Instance.Connection.TipoDatabase)
            {
                /*case FrameworkDBType.OracleClient:
                case FrameworkDBType.ODP10:
                case FrameworkDBType.PostgreSQL:
                case FrameworkDBType.SQLServer:
                    break;
                case FrameworkDBType.Access:
                    using (OpenFileDialog dial = new OpenFileDialog())
                    {
                        dial.DefaultExt = "mdb";
                        dial.Filter = "Database Access|*.mdb|Applicazione Access|*.accdb";
                        if (dial.ShowDialog() == DialogResult.OK)
                        {
                            txtDatabase.Text = dial.FileName;
                        }
                    }

                    break;
                case FrameworkDBType.SQLServerFile:
                    using (OpenFileDialog dial = new OpenFileDialog())
                    {
                        dial.DefaultExt = "mdf";
                        dial.Filter = "SQL Server data file|*.mdf";
                        if (dial.ShowDialog() == DialogResult.OK)
                        {
                            txtDatabase.Text = dial.FileName;
                        }
                    }
                    break;
                case FrameworkDBType.SQLite:
                    using (OpenFileDialog dial = new OpenFileDialog())
                    {

                        dial.DefaultExt = "db";
                        dial.Filter = "SQLLite Database |*.db";
                        if (dial.ShowDialog() == DialogResult.OK)
                        {
                            txtDatabase.Text = dial.FileName;
                        }
                    }
                    break;
                case FrameworkDBType.SqlServerCE:
                    using (OpenFileDialog dial = new OpenFileDialog())
                    {
                        dial.DefaultExt = "sdf";
                        dial.Filter = "Database SQL Compact Edition|*.sdf";
                        if (dial.ShowDialog() == DialogResult.OK)
                        {
                            txtDatabase.Text = dial.FileName;
                        }
                    }

                    break;*/
                case "PostgreSQLProvider":
                    break;
                default:
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Params.Instance.Connection.Database = txtDatabase.Text;
            Params.Instance.Connection.Owner = txtOwner.Text;
            Params.Instance.Connection.Password = txtPassword.Text;
            Params.Instance.Connection.Server = txtServer.Text;
            Params.Instance.Connection.Utente = txtUtente.Text;

            DBStructure.GenerateConnectionString();

            if (SaveConfig != null)
            {
                SaveConfig(this, null);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            if (Cancel != null)
            {
                Cancel(this, null);
            }
        }
    }
}
