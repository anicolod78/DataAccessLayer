using IT.TnDigit.ORM.Configurator.Classi;
using System;
using System.Data;
using System.Windows.Forms;

namespace IT.TnDigit.ORM.Configurator.Configurations
{
    public partial class ucOBJSelect : UserControl, IucBase
    {
        public event EventHandler SaveConfig;
        public event EventHandler Cancel;

        public ucOBJSelect()
        {
            InitializeComponent();
            if (Params.Instance.Objects.Tables == null)
                Params.Instance.Objects.Tables = [];
            if (Params.Instance.Objects.Views == null)
                Params.Instance.Objects.Views = [];
            if (Params.Instance.Objects.Functions == null)
                Params.Instance.Objects.Functions = [];
        }

        private void ucDBConfig_Load(object sender, EventArgs e)
        {
            string itemName = "";

            chlTabelle.Items.Clear();
            foreach (DataRow r in DBStructure.GetTables().Rows)
            {
                itemName = r["Table_Name"].ToString();
                if (!Params.Instance.Objects.Tables.ContainsKey(itemName))
                    chlTabelle.Items.Add(new Selections.Table() { Name = itemName }, false);
                else
                    chlTabelle.Items.Add(Params.Instance.Objects.Tables[itemName], true);
            }

            chlViste.Items.Clear();
            foreach (DataRow r in DBStructure.GetViews().Rows)
            {
                itemName = r["Table_Name"].ToString();
                chlViste.Items.Add(itemName, Params.Instance.Objects.Views.Contains(itemName));
            }

            chlStoredProcedures.Items.Clear();
            foreach (DataRow r in DBStructure.GetProcedures().Rows)
            {
                if (!(r["PACKAGE_NAME"] is DBNull) && r["PACKAGE_NAME"].ToString() != "")
                    itemName = r["PACKAGE_NAME"].ToString() + "." + r["OBJECT_NAME"].ToString();
                else
                    itemName = r["OBJECT_NAME"].ToString();

                chlStoredProcedures.Items.Add(itemName, Params.Instance.Objects.Functions.Contains(itemName));

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Params.Instance.Objects.Tables.Clear();
            foreach (var item in chlTabelle.CheckedItems)
            {
                Selections.Table t = item as Selections.Table;
                Params.Instance.Objects.Tables.Add(t.Name, t);
            }
            Params.Instance.Objects.Views.Clear();
            foreach (var item in chlViste.CheckedItems)
            {
                Params.Instance.Objects.Views.Add(item.ToString());
            }
            Params.Instance.Objects.Functions.Clear();
            foreach (var item in chlStoredProcedures.CheckedItems)
            {
                Params.Instance.Objects.Functions.Add(item.ToString());
            }


            SaveConfig?.Invoke(this, null);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cancel?.Invoke(this, null);
        }

        private void btnTableSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chlTabelle.Items.Count; i++)
            {
                chlTabelle.SetItemChecked(i, true);
            }
        }

        private void btnViewSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chlViste.Items.Count; i++)
            {
                chlViste.SetItemChecked(i, true);
            }
        }

        private void btnProcedureSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chlStoredProcedures.Items.Count; i++)
            {
                chlStoredProcedures.SetItemChecked(i, true);
            }
        }

        private void btnTableDeselect_Click(object sender, EventArgs e)
        {
            foreach (int i in chlTabelle.CheckedIndices)
            {
                chlTabelle.SetItemChecked(i, false);
            }
        }

        private void btnViewDeselect_Click(object sender, EventArgs e)
        {
            foreach (int i in chlViste.CheckedIndices)
            {
                chlViste.SetItemChecked(i, false);
            }

        }

        private void btnProcedureDeselect_Click(object sender, EventArgs e)
        {
            foreach (int i in chlStoredProcedures.CheckedIndices)
            {
                chlStoredProcedures.SetItemChecked(i, false);
            }
        }

        private void mnuAdvancedConfig_Click(object sender, EventArgs e)
        {
            //TODO: aprire box con lista campi e attributi associati/associabili
            //preselezionare PK, MaxLength e per i campi standard (AllowDateTime , DateCreation, DateLastModified)
            Selections.Table obj = chlTabelle.SelectedItem as Selections.Table;
            if (obj != null)
            {
                obj.AdvancedConfig = true;
                chlTabelle.Refresh();
            }
        }

        private void chlTabelle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int i = chlTabelle.IndexFromPoint(e.Location);

                if (chlTabelle.CheckedIndices.Contains(i))
                {
                    chlTabelle.SelectedIndex = i;
                    contextMenuStrip1.Show(chlTabelle, e.Location);
                }
            }
        }

    }
}
