using IT.TnDigit.ORM.ClientController;
using IT.TnDigit.ORM.Configurator.Classi;
using IT.TnDigit.ORM.Configurator.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT.TnDigit.ORM.Configurator
{
    public partial class frmMain : Form
    {
        UserControl currentUC = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            pnlOBJ.Enabled = false;
            btnStartGeneration.Enabled = false;

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("-f"))
                {
                    string file = arg.Remove(0, 2).Trim();

                    if (File.Exists(file) == false)
                        return;

                    LoadConfig(file);
                }
            }
        }

        private void btnStartGeneration_Click(object sender, EventArgs e)
        {
            if (DBStructure.CheckConnection() != null)
            {
                MessageBox.Show("Connessione alla base dati non attiva.");
                return;
            }

            if (Params.Instance.Domain.FilePath == "")
            {
                MessageBox.Show("Percorso di output non impostato.");
                return;
            }

            string text = btnStartGeneration.Text;
            btnStartGeneration.Enabled = false;
            btnStartGeneration.Text = "Attendere...";
            toolStrip1.Enabled = false;

            int numOggetti = Params.Instance.Objects.Tables.Count +
                Params.Instance.Objects.Views.Count +
                Params.Instance.Objects.Functions.Count;

            var pnl = new Panel();
            splitContainer2.Panel1.Controls.Add(pnl);
            pnl.Dock = DockStyle.Fill;
            pnl.BackColor = Color.Gray;
            pnl.BringToFront();
            Application.DoEvents();

            IT.InfoTn.Win.Controls.GroupBox grp = new IT.InfoTn.Win.Controls.GroupBox();
            grp.ColoreTestoLegend = Color.White;
            grp.ColoreSfondoLegend = Color.DimGray;
            grp.ColoreBordoLegend = Color.White;
            grp.Raggio = 5;
            grp.Text = string.Format("Generazione di {0} oggetti", numOggetti);
            grp.Width = pnl.Width / 2;
            grp.Left = (pnl.Width - grp.Width) / 2;
            grp.Height = pnl.Height / 3;
            grp.Top = pnl.Height / 3;
            pnl.Controls.Add(grp);

            ProgressBar prb = new ProgressBar();
            prb.Maximum = numOggetti;
            prb.Value = 0;
            prb.Width = grp.Width - grp.Padding.Left - grp.Padding.Right - 10;
            prb.Left = grp.Padding.Left + 5;
            prb.Top = (grp.Height - grp.Padding.Top - grp.Padding.Bottom - prb.Height) / 2;
            grp.Controls.Add(prb);

            Label lblCurrentObject = new Label();
            lblCurrentObject.Text = "...";
            lblCurrentObject.TextAlign = ContentAlignment.TopCenter;
            lblCurrentObject.AutoSize = false;
            lblCurrentObject.AutoEllipsis = true;
            lblCurrentObject.Width = grp.Width - grp.Padding.Left - grp.Padding.Right - 10;
            lblCurrentObject.Left = grp.Padding.Left + 5;
            lblCurrentObject.Top = prb.Top + 5 + prb.Height;
            grp.Controls.Add(lblCurrentObject);
            Application.DoEvents();

            int i = 1;
            int s = 0;

            foreach (var item in Params.Instance.Objects.Tables)
            {
                prb.Value = i;
                lblCurrentObject.Text = string.Format("Table {0}", item.Value);
                lblCurrentObject.Refresh();
                Application.DoEvents();
                if (Tables.WriteObject(item.Value))
                    i++;
                else
                    s++;
                Application.DoEvents();
            }
            foreach (var item in Params.Instance.Objects.Views)
            {
                prb.Value = i;
                lblCurrentObject.Text = string.Format("View {0}", item);
                lblCurrentObject.Refresh();
                Application.DoEvents();
                if (Views.WriteObject(item))
                    i++;
                else
                    s++;
                Application.DoEvents();
            }
            foreach (var item in Params.Instance.Objects.Functions)
            {
                prb.Value = i;
                lblCurrentObject.Text = string.Format("Function {0}", item);
                lblCurrentObject.Refresh();
                Application.DoEvents();
                if (Procedures.WriteObject(item))
                    i++;
                else
                    s++;
                Application.DoEvents();
            }

            if (s == 0)
            {
                MessageBox.Show(
                    "Generazione completata con successo.",
                    "Generazione oggetti",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    string.Format("Generazione completata parzialmente:{0}{1} oggetti su {2} non sono stati generati perché in sola lettura (check-out non effettuato).", Environment.NewLine, s, s + i),
                    "Generazione oggetti",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            btnStartGeneration.Enabled = true;
            btnStartGeneration.Text = text;
            toolStrip1.Enabled = true;
            pnl.Dispose();
        }

        void uc_Cancel(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Remove(currentUC);
            currentUC = null;
        }

        private IucBase CreateScreen(UserControl uc, Color color)
        {
            splitContainer1.Panel2.Controls.Add(uc);
            uc.BackColor = color;
            uc.Dock = DockStyle.Fill;
            ((IucBase)uc).Cancel += new EventHandler(uc_Cancel);
            uc.BringToFront();
            Application.DoEvents();
            currentUC = uc;

            return uc as IucBase;
        }

        #region DATABASE CONFIG
        private void lnkDBConfiguration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ucDB = CreateScreen(new ucDBConfig(), pnlDB.ColoreSfondoFieldset);
            ucDB.SaveConfig += new EventHandler(ucDB_SaveConfig);
        }

        void ucDB_SaveConfig(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Remove(currentUC);
            currentUC = null;
            //update info box
            UpdateInterfaceInfos();
        }
        #endregion

        #region PROJECT CONFIG
        private void lnkPRJConfiguration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ucPRJ = CreateScreen(new ucPRJConfig(), pnlPRJ.ColoreSfondoFieldset);
            ucPRJ.SaveConfig += new EventHandler(ucPRJ_SaveConfig);
        }
        void ucPRJ_SaveConfig(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Remove(currentUC);
            currentUC = null;
            //update info box
            UpdateInterfaceInfos();
        }
        #endregion

        #region OBJECT SELECTION
        private void lnkOBJSelection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ucOBJ = CreateScreen(new ucOBJSelect(), pnlOBJ.ColoreSfondoFieldset);
            ucOBJ.SaveConfig += new EventHandler(ucOBJ_SaveConfig);
            ucOBJ.Cancel += new EventHandler(ucOBJ_Cancel);
        }

        void ucOBJ_SaveConfig(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Remove(currentUC);
            currentUC = null;
            //update info box
            UpdateInterfaceInfos();
        }

        void ucOBJ_Cancel(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Remove(currentUC);
            currentUC = null;
        }
        #endregion

        #region RULE CREATION
        private void lnkRULCreation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ucRUL = CreateScreen(new ucRULCreate(), pnlRUL.ColoreSfondoFieldset);
            ucRUL.SaveConfig += new EventHandler(ucRUL_SaveConfig);
        }

        void ucRUL_SaveConfig(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Remove(currentUC);
            currentUC = null;
            //update info box
        }
        #endregion

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            btnStartGeneration.Left = (splitContainer2.Panel2.Width - btnStartGeneration.Width) / 2;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

            btnStartGeneration.Left = (splitContainer2.Panel2.Width - btnStartGeneration.Width) / 2;
        }


        private void UpdateInterfaceInfos()
        {
            lblDBType.Text = Params.Instance.Connection.TipoDatabase.ToString();
            lblDBName.Text = Params.Instance.Connection.Database; ;
            if (Params.Instance.Connection.TipoDatabase == null)
            {
                lblDBStatus.Text = "No configuration";
                toolTip1.SetToolTip(lblDBStatus, "");
                pnlOBJ.Enabled = false;
                btnStartGeneration.Enabled = false;
            }
            else
            {
                var ex = DBStructure.CheckConnection();
                if (ex == null)
                {
                    lblDBStatus.Text = "OK";
                    toolTip1.SetToolTip(lblDBStatus, "");
                }
                else
                {
                    lblDBStatus.Text = "Error";
                    toolTip1.SetToolTip(lblDBStatus, ex.Message);
                }

                pnlOBJ.Enabled = (ex == null);
                btnStartGeneration.Enabled = (ex == null && Params.Instance.Domain.FilePath != "");
            }

            lblPRJName.Text = Params.Instance.Domain.Progetto;
            lblPRJNamespace.Text = Params.Instance.Domain.NameSpace;


            lblOBJTables.Text = string.Format("{0} tables", Params.Instance.Objects.Tables != null ? Params.Instance.Objects.Tables.Count : 0);
            lblOBJViews.Text = string.Format("{0} views", Params.Instance.Objects.Views != null ? Params.Instance.Objects.Views.Count : 0);
            lblOBJFunctions.Text = string.Format("{0} functions", Params.Instance.Objects.Functions != null ? Params.Instance.Objects.Functions.Count : 0);

        }




        private void btnOpenProjectConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.DefaultExt = "fwk";
            dial.Filter = "InfoTn framework config|*.fwk";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                string file = dial.FileName;
                LoadConfig(file);
            }
        }

        private void LoadConfig(string file)
        {
            System.Configuration.ProjectConfigurationSettings.configPath = file;

            System.Configuration.ProjectConfigurationSettings.Reload();
            Params.Instance.Connection.TipoDatabase = System.Configuration.ProjectConfigurationSettings.AppSettings["DbType"];
            Params.Instance.Connection.Server = System.Configuration.ProjectConfigurationSettings.AppSettings["Server"];
            Params.Instance.Connection.Database = System.Configuration.ProjectConfigurationSettings.AppSettings["Database"];
            Params.Instance.Connection.Owner = System.Configuration.ProjectConfigurationSettings.AppSettings["OwnerTabelle"];
            Params.Instance.Connection.Utente = System.Configuration.ProjectConfigurationSettings.AppSettings["Utente"];
            Params.Instance.Connection.Password = HelperEncryptC.DecryptText(System.Configuration.ProjectConfigurationSettings.AppSettings["Password"]);
            Params.Instance.Connection.ConnectionString = string.Empty;
            try
            {
                Params.Instance.Connection.ConnectionString = HelperEncryptC.DecryptText(System.Configuration.ProjectConfigurationSettings.AppSettings["ConnectionString"]);
            }
            catch
            {
            }
            if (Params.Instance.Connection.ConnectionString == string.Empty)
                DBStructure.GenerateConnectionString();

            Params.Instance.Domain.Progetto = string.Empty;
            try
            {
                Params.Instance.Domain.Progetto = System.Configuration.ProjectConfigurationSettings.AppSettings["Progetto"];
            }
            catch
            {
            }

            Params.Instance.Domain.NameSpace = System.Configuration.ProjectConfigurationSettings.AppSettings["Namespace"];
            Params.Instance.Domain.BaseClassTables = System.Configuration.ProjectConfigurationSettings.AppSettings["ClasseBaseTabelle"];
            Params.Instance.Domain.BaseClassViews = System.Configuration.ProjectConfigurationSettings.AppSettings["ClasseBaseViste"];
            Params.Instance.Domain.BaseClassProcs = System.Configuration.ProjectConfigurationSettings.AppSettings["ClasseBaseProcedure"];
            Params.Instance.Domain.FilePath = System.Configuration.ProjectConfigurationSettings.AppSettings["DestinationPath"];
            Params.Instance.Domain.ParamsDefaultValue = System.Configuration.ProjectConfigurationSettings.AppSettings["ParameterDefaultValue"];
            Params.Instance.Domain.NumericiSpecifici = false;
            try
            {
                Params.Instance.Domain.NumericiSpecifici = bool.Parse(System.Configuration.ProjectConfigurationSettings.AppSettings["NumericiSpecifici"]);
            }
            catch { }

            Params.Instance.Objects.Tables.Clear();
            Params.Instance.Objects.Views.Clear();
            Params.Instance.Objects.Functions.Clear();
            try
            {
                Params.Instance.Objects = Selections.Deserialize(System.Configuration.ProjectConfigurationSettings.AppSettings["Oggetti"]);
            }
            catch { }
            UpdateInterfaceInfos();
        }

        private void btnSaveProjectConfig_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<configuration>");
            sb.AppendLine("\t<appSettings>");
            sb.AppendFormat("\t\t<add key=\"DbType\" value=\"{0}\" />\r\n", Params.Instance.Connection.TipoDatabase);
            sb.AppendFormat("\t\t<add key=\"Server\" value=\"{0}\" />\r\n", Params.Instance.Connection.Server);
            sb.AppendFormat("\t\t<add key=\"Database\" value=\"{0}\" />\r\n", Params.Instance.Connection.Database);
            sb.AppendFormat("\t\t<add key=\"OwnerTabelle\" value=\"{0}\" />\r\n", Params.Instance.Connection.Owner);
            sb.AppendFormat("\t\t<add key=\"Utente\" value=\"{0}\" />\r\n", Params.Instance.Connection.Utente);
            sb.AppendFormat("\t\t<add key=\"Password\" value=\"{0}\" />\r\n", HelperEncryptC.EncryptText(Params.Instance.Connection.Password));
            sb.AppendFormat("\t\t<add key=\"ConnectionString\" value=\"{0}\" />\r\n", HelperEncryptC.EncryptText(Params.Instance.Connection.ConnectionString));

            sb.AppendFormat("\t\t<add key=\"Progetto\" value=\"{0}\" />\r\n", Params.Instance.Domain.Progetto);
            sb.AppendFormat("\t\t<add key=\"Namespace\" value=\"{0}\" />\r\n", Params.Instance.Domain.NameSpace);
            sb.AppendFormat("\t\t<add key=\"ClasseBaseTabelle\" value=\"{0}\" />\r\n", Params.Instance.Domain.BaseClassTables);
            sb.AppendFormat("\t\t<add key=\"ClasseBaseViste\" value=\"{0}\" />\r\n", Params.Instance.Domain.BaseClassViews);
            sb.AppendFormat("\t\t<add key=\"ClasseBaseProcedure\" value=\"{0}\" />\r\n", Params.Instance.Domain.BaseClassProcs);
            sb.AppendFormat("\t\t<add key=\"DestinationPath\" value=\"{0}\" />\r\n", Params.Instance.Domain.FilePath);
            sb.AppendFormat("\t\t<add key=\"ParameterDefaultValue\" value=\"{0}\" />\r\n", Params.Instance.Domain.ParamsDefaultValue);
            sb.AppendFormat("\t\t<add key=\"NumericiSpecifici\" value=\"{0}\" />\r\n", Params.Instance.Domain.NumericiSpecifici.ToString());

            sb.AppendFormat("\t\t<add key=\"Oggetti\" value=\"{0}\" />\r\n", Params.Instance.Objects.Serialize());

            sb.AppendLine("\t</appSettings>");
            sb.AppendLine("</configuration>");

            SaveFileDialog diag = new SaveFileDialog();
            diag.AddExtension = true;
            diag.DefaultExt = "fwk";
            diag.Filter = "InfoTn framework config|*.fwk";

            string file = "";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                file = diag.FileName;
            }
            else
            {
                return;
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter(file);
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();

            MessageBox.Show("Configurazione progetto salvata con successo.");
        }

        private void btnNewProjectConfig_Click(object sender, EventArgs e)
        {
            Params.Instance.Connection.TipoDatabase = null;
            Params.Instance.Connection.Server = "";
            Params.Instance.Connection.Database = "";
            Params.Instance.Connection.Owner = "";
            Params.Instance.Connection.Utente = "";
            Params.Instance.Connection.Password = "";
            Params.Instance.Connection.ConnectionString = "";

            Params.Instance.Domain.Progetto = string.Empty;
            Params.Instance.Domain.NameSpace = "DataTypes";
            Params.Instance.Domain.BaseClassTables = "FrameworkObject";
            Params.Instance.Domain.BaseClassViews = "FrameworkObject";
            Params.Instance.Domain.BaseClassProcs = "FrameworkStoredProcedure";
            Params.Instance.Domain.ParamsDefaultValue = "null";
            Params.Instance.Domain.FilePath = string.Empty;

            Params.Instance.Objects.Tables.Clear();
            Params.Instance.Objects.Views.Clear();
            Params.Instance.Objects.Functions.Clear();

            UpdateInterfaceInfos();
        }

    }
}
