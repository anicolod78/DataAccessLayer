using IT.TnDigit.ORM.Configurator.Classi;
using System;
using System.Windows.Forms;

namespace IT.TnDigit.ORM.Configurator.Configurations
{
    public partial class ucPRJConfig : UserControl, IucBase
    {
        public event EventHandler SaveConfig;
        public event EventHandler Cancel;

        public ucPRJConfig()
        {
            InitializeComponent();
        }

        private void ucDBConfig_Load(object sender, EventArgs e)
        {
            txtProgetto.Text = Params.Instance.Domain.Progetto;
            txtNameSpace.Text = Params.Instance.Domain.NameSpace;
            txtBaseClass.Text = Params.Instance.Domain.BaseClassTables;
            txtBaseClassView.Text = Params.Instance.Domain.BaseClassViews;
            txtBaseClassProcedure.Text = Params.Instance.Domain.BaseClassProcs;
            txtdefaultvalue.Text = Params.Instance.Domain.ParamsDefaultValue;
            chkNumericiSpecifici.Checked = Params.Instance.Domain.NumericiSpecifici;
            txtPercorsoOutput.Text = Params.Instance.Domain.FilePath;
        }

        private void btnPercorsoOutput_Click(object sender, EventArgs e)
        {
            if (Params.Instance.Domain.FilePath != string.Empty)
            {
                folderBrowserDialog1.SelectedPath = Params.Instance.Domain.FilePath;
            }
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPercorsoOutput.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            Params.Instance.Domain.Progetto = txtProgetto.Text;
            Params.Instance.Domain.NameSpace = txtNameSpace.Text;
            Params.Instance.Domain.BaseClassTables = txtBaseClass.Text;
            Params.Instance.Domain.BaseClassViews = txtBaseClassView.Text;
            Params.Instance.Domain.BaseClassProcs = txtBaseClassProcedure.Text;
            Params.Instance.Domain.ParamsDefaultValue = txtdefaultvalue.Text;
            Params.Instance.Domain.NumericiSpecifici = chkNumericiSpecifici.Checked;
            Params.Instance.Domain.FilePath = txtPercorsoOutput.Text;

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
