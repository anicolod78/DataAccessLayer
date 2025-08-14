using IT.TnDigit.ORM.Configurator.Classi;
using System;
using System.Windows.Forms;

namespace IT.TnDigit.ORM.Configurator.Configurations
{
    public partial class ucRULCreate : UserControl, IucBase
    {
        public event EventHandler SaveConfig;
        public event EventHandler Cancel;

        public ucRULCreate()
        {
            InitializeComponent();
        }

        private void ucDBConfig_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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
