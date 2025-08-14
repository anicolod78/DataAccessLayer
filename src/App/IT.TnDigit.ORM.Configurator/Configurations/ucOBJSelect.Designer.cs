namespace IT.TnDigit.ORM.Configurator.Configurations
{
    partial class ucOBJSelect
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnSave = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            tabObjects = new System.Windows.Forms.TabControl();
            tabTabelle = new System.Windows.Forms.TabPage();
            btnTableDeselect = new System.Windows.Forms.Button();
            btnTableSelect = new System.Windows.Forms.Button();
            chlTabelle = new System.Windows.Forms.CheckedListBox();
            tabViste = new System.Windows.Forms.TabPage();
            btnViewDeselect = new System.Windows.Forms.Button();
            btnViewSelect = new System.Windows.Forms.Button();
            chlViste = new System.Windows.Forms.CheckedListBox();
            tabFunzioni = new System.Windows.Forms.TabPage();
            btnProcedureDeselect = new System.Windows.Forms.Button();
            btnProcedureSelect = new System.Windows.Forms.Button();
            chlStoredProcedures = new System.Windows.Forms.CheckedListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            mnuAdvancedConfig = new System.Windows.Forms.ToolStripMenuItem();
            tabObjects.SuspendLayout();
            tabTabelle.SuspendLayout();
            tabViste.SuspendLayout();
            tabFunzioni.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnSave.Location = new System.Drawing.Point(532, 473);
            btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(100, 35);
            btnSave.TabIndex = 24;
            btnSave.Text = "&CONFERMA";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdCancel.Location = new System.Drawing.Point(413, 473);
            cmdCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 35);
            cmdCancel.TabIndex = 25;
            cmdCancel.Text = "&ANNULLA";
            cmdCancel.UseVisualStyleBackColor = true;
            cmdCancel.Click += cmdCancel_Click;
            // 
            // tabObjects
            // 
            tabObjects.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabObjects.Controls.Add(tabTabelle);
            tabObjects.Controls.Add(tabViste);
            tabObjects.Controls.Add(tabFunzioni);
            tabObjects.Location = new System.Drawing.Point(19, 25);
            tabObjects.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabObjects.Name = "tabObjects";
            tabObjects.SelectedIndex = 0;
            tabObjects.Size = new System.Drawing.Size(617, 442);
            tabObjects.TabIndex = 26;
            // 
            // tabTabelle
            // 
            tabTabelle.Controls.Add(btnTableDeselect);
            tabTabelle.Controls.Add(btnTableSelect);
            tabTabelle.Controls.Add(chlTabelle);
            tabTabelle.Location = new System.Drawing.Point(4, 29);
            tabTabelle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabTabelle.Name = "tabTabelle";
            tabTabelle.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabTabelle.Size = new System.Drawing.Size(609, 409);
            tabTabelle.TabIndex = 0;
            tabTabelle.Text = "Tabelle";
            tabTabelle.UseVisualStyleBackColor = true;
            // 
            // btnTableDeselect
            // 
            btnTableDeselect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnTableDeselect.BackColor = System.Drawing.Color.WhiteSmoke;
            btnTableDeselect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnTableDeselect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnTableDeselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnTableDeselect.Location = new System.Drawing.Point(549, 9);
            btnTableDeselect.Margin = new System.Windows.Forms.Padding(0);
            btnTableDeselect.Name = "btnTableDeselect";
            btnTableDeselect.Size = new System.Drawing.Size(23, 26);
            btnTableDeselect.TabIndex = 15;
            btnTableDeselect.Text = "-";
            btnTableDeselect.UseCompatibleTextRendering = true;
            btnTableDeselect.UseVisualStyleBackColor = false;
            btnTableDeselect.Click += btnTableDeselect_Click;
            // 
            // btnTableSelect
            // 
            btnTableSelect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnTableSelect.BackColor = System.Drawing.Color.WhiteSmoke;
            btnTableSelect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnTableSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnTableSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnTableSelect.Location = new System.Drawing.Point(576, 9);
            btnTableSelect.Margin = new System.Windows.Forms.Padding(0);
            btnTableSelect.Name = "btnTableSelect";
            btnTableSelect.Size = new System.Drawing.Size(23, 26);
            btnTableSelect.TabIndex = 14;
            btnTableSelect.Text = "+";
            btnTableSelect.UseCompatibleTextRendering = true;
            btnTableSelect.UseVisualStyleBackColor = false;
            btnTableSelect.Click += btnTableSelect_Click;
            // 
            // chlTabelle
            // 
            chlTabelle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            chlTabelle.BackColor = System.Drawing.Color.White;
            chlTabelle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            chlTabelle.FormattingEnabled = true;
            chlTabelle.Location = new System.Drawing.Point(8, 42);
            chlTabelle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chlTabelle.Name = "chlTabelle";
            chlTabelle.Size = new System.Drawing.Size(591, 330);
            chlTabelle.TabIndex = 13;
            chlTabelle.MouseDown += chlTabelle_MouseDown;
            // 
            // tabViste
            // 
            tabViste.Controls.Add(btnViewDeselect);
            tabViste.Controls.Add(btnViewSelect);
            tabViste.Controls.Add(chlViste);
            tabViste.Location = new System.Drawing.Point(4, 29);
            tabViste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabViste.Name = "tabViste";
            tabViste.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabViste.Size = new System.Drawing.Size(609, 409);
            tabViste.TabIndex = 1;
            tabViste.Text = "Viste";
            tabViste.UseVisualStyleBackColor = true;
            // 
            // btnViewDeselect
            // 
            btnViewDeselect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnViewDeselect.BackColor = System.Drawing.Color.WhiteSmoke;
            btnViewDeselect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnViewDeselect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnViewDeselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnViewDeselect.Location = new System.Drawing.Point(549, 9);
            btnViewDeselect.Margin = new System.Windows.Forms.Padding(0);
            btnViewDeselect.Name = "btnViewDeselect";
            btnViewDeselect.Size = new System.Drawing.Size(23, 26);
            btnViewDeselect.TabIndex = 15;
            btnViewDeselect.Text = "-";
            btnViewDeselect.UseCompatibleTextRendering = true;
            btnViewDeselect.UseVisualStyleBackColor = false;
            btnViewDeselect.Click += btnViewDeselect_Click;
            // 
            // btnViewSelect
            // 
            btnViewSelect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnViewSelect.BackColor = System.Drawing.Color.WhiteSmoke;
            btnViewSelect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnViewSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnViewSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnViewSelect.Location = new System.Drawing.Point(576, 9);
            btnViewSelect.Margin = new System.Windows.Forms.Padding(0);
            btnViewSelect.Name = "btnViewSelect";
            btnViewSelect.Size = new System.Drawing.Size(23, 26);
            btnViewSelect.TabIndex = 14;
            btnViewSelect.Text = "+";
            btnViewSelect.UseCompatibleTextRendering = true;
            btnViewSelect.UseVisualStyleBackColor = false;
            btnViewSelect.Click += btnViewSelect_Click;
            // 
            // chlViste
            // 
            chlViste.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            chlViste.BackColor = System.Drawing.Color.White;
            chlViste.BorderStyle = System.Windows.Forms.BorderStyle.None;
            chlViste.FormattingEnabled = true;
            chlViste.Location = new System.Drawing.Point(8, 42);
            chlViste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chlViste.Name = "chlViste";
            chlViste.Size = new System.Drawing.Size(591, 330);
            chlViste.TabIndex = 13;
            // 
            // tabFunzioni
            // 
            tabFunzioni.Controls.Add(btnProcedureDeselect);
            tabFunzioni.Controls.Add(btnProcedureSelect);
            tabFunzioni.Controls.Add(chlStoredProcedures);
            tabFunzioni.Location = new System.Drawing.Point(4, 29);
            tabFunzioni.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabFunzioni.Name = "tabFunzioni";
            tabFunzioni.Size = new System.Drawing.Size(609, 409);
            tabFunzioni.TabIndex = 2;
            tabFunzioni.Text = "Funzioni";
            tabFunzioni.UseVisualStyleBackColor = true;
            // 
            // btnProcedureDeselect
            // 
            btnProcedureDeselect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnProcedureDeselect.BackColor = System.Drawing.Color.WhiteSmoke;
            btnProcedureDeselect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnProcedureDeselect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnProcedureDeselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnProcedureDeselect.Location = new System.Drawing.Point(549, 9);
            btnProcedureDeselect.Margin = new System.Windows.Forms.Padding(0);
            btnProcedureDeselect.Name = "btnProcedureDeselect";
            btnProcedureDeselect.Size = new System.Drawing.Size(23, 26);
            btnProcedureDeselect.TabIndex = 15;
            btnProcedureDeselect.Text = "-";
            btnProcedureDeselect.UseCompatibleTextRendering = true;
            btnProcedureDeselect.UseVisualStyleBackColor = false;
            btnProcedureDeselect.Click += btnProcedureDeselect_Click;
            // 
            // btnProcedureSelect
            // 
            btnProcedureSelect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnProcedureSelect.BackColor = System.Drawing.Color.WhiteSmoke;
            btnProcedureSelect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnProcedureSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnProcedureSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnProcedureSelect.Location = new System.Drawing.Point(576, 9);
            btnProcedureSelect.Margin = new System.Windows.Forms.Padding(0);
            btnProcedureSelect.Name = "btnProcedureSelect";
            btnProcedureSelect.Size = new System.Drawing.Size(23, 26);
            btnProcedureSelect.TabIndex = 14;
            btnProcedureSelect.Text = "+";
            btnProcedureSelect.UseCompatibleTextRendering = true;
            btnProcedureSelect.UseVisualStyleBackColor = false;
            btnProcedureSelect.Click += btnProcedureSelect_Click;
            // 
            // chlStoredProcedures
            // 
            chlStoredProcedures.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            chlStoredProcedures.BackColor = System.Drawing.Color.White;
            chlStoredProcedures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            chlStoredProcedures.FormattingEnabled = true;
            chlStoredProcedures.Location = new System.Drawing.Point(8, 42);
            chlStoredProcedures.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chlStoredProcedures.Name = "chlStoredProcedures";
            chlStoredProcedures.Size = new System.Drawing.Size(591, 330);
            chlStoredProcedures.TabIndex = 13;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuAdvancedConfig });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Size = new System.Drawing.Size(218, 28);
            // 
            // mnuAdvancedConfig
            // 
            mnuAdvancedConfig.Name = "mnuAdvancedConfig";
            mnuAdvancedConfig.Size = new System.Drawing.Size(217, 24);
            mnuAdvancedConfig.Text = "Configurazione avanzata";
            mnuAdvancedConfig.Click += mnuAdvancedConfig_Click;
            // 
            // ucOBJSelect
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabObjects);
            Controls.Add(cmdCancel);
            Controls.Add(btnSave);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "ucOBJSelect";
            Size = new System.Drawing.Size(653, 518);
            Load += ucDBConfig_Load;
            tabObjects.ResumeLayout(false);
            tabTabelle.ResumeLayout(false);
            tabViste.ResumeLayout(false);
            tabFunzioni.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabObjects;
        private System.Windows.Forms.TabPage tabTabelle;
        private System.Windows.Forms.TabPage tabViste;
        private System.Windows.Forms.TabPage tabFunzioni;
        private System.Windows.Forms.Button btnTableDeselect;
        private System.Windows.Forms.Button btnTableSelect;
        private System.Windows.Forms.CheckedListBox chlTabelle;
        private System.Windows.Forms.Button btnViewDeselect;
        private System.Windows.Forms.Button btnViewSelect;
        private System.Windows.Forms.CheckedListBox chlViste;
        private System.Windows.Forms.Button btnProcedureDeselect;
        private System.Windows.Forms.Button btnProcedureSelect;
        private System.Windows.Forms.CheckedListBox chlStoredProcedures;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuAdvancedConfig;
    }
}
