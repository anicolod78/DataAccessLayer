namespace IT.TnDigit.ORM.Configurator.Configurations
{
    partial class ucPRJConfig
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
            btnSave = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            label9 = new System.Windows.Forms.Label();
            txtdefaultvalue = new System.Windows.Forms.TextBox();
            txtBaseClassProcedure = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            txtPercorsoOutput = new System.Windows.Forms.Label();
            btnPercorsoOutput = new System.Windows.Forms.Button();
            label7 = new System.Windows.Forms.Label();
            txtBaseClassView = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            txtBaseClass = new System.Windows.Forms.TextBox();
            BaseClass = new System.Windows.Forms.Label();
            txtNameSpace = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            chkNumericiSpecifici = new System.Windows.Forms.CheckBox();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            txtProgetto = new System.Windows.Forms.TextBox();
            lblName = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnSave.Location = new System.Drawing.Point(538, 331);
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
            cmdCancel.Location = new System.Drawing.Point(430, 331);
            cmdCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 35);
            cmdCancel.TabIndex = 25;
            cmdCancel.Text = "&ANNULLA";
            cmdCancel.UseVisualStyleBackColor = true;
            cmdCancel.Click += cmdCancel_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = System.Drawing.Color.Transparent;
            label9.ForeColor = System.Drawing.Color.White;
            label9.Location = new System.Drawing.Point(15, 226);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(171, 20);
            label9.TabIndex = 38;
            label9.Text = "Valore default parametri";
            // 
            // txtdefaultvalue
            // 
            txtdefaultvalue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtdefaultvalue.Location = new System.Drawing.Point(180, 223);
            txtdefaultvalue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtdefaultvalue.Name = "txtdefaultvalue";
            txtdefaultvalue.Size = new System.Drawing.Size(99, 27);
            txtdefaultvalue.TabIndex = 37;
            txtdefaultvalue.Text = "null";
            txtdefaultvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBaseClassProcedure
            // 
            txtBaseClassProcedure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtBaseClassProcedure.Location = new System.Drawing.Point(180, 182);
            txtBaseClassProcedure.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtBaseClassProcedure.Name = "txtBaseClassProcedure";
            txtBaseClassProcedure.Size = new System.Drawing.Size(267, 27);
            txtBaseClassProcedure.TabIndex = 35;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = System.Drawing.Color.Transparent;
            label8.ForeColor = System.Drawing.Color.White;
            label8.Location = new System.Drawing.Point(15, 185);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(157, 20);
            label8.TabIndex = 36;
            label8.Text = "Classe base procedure";
            // 
            // txtPercorsoOutput
            // 
            txtPercorsoOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPercorsoOutput.AutoEllipsis = true;
            txtPercorsoOutput.BackColor = System.Drawing.Color.White;
            txtPercorsoOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPercorsoOutput.Location = new System.Drawing.Point(180, 266);
            txtPercorsoOutput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            txtPercorsoOutput.Name = "txtPercorsoOutput";
            txtPercorsoOutput.Size = new System.Drawing.Size(410, 31);
            txtPercorsoOutput.TabIndex = 34;
            txtPercorsoOutput.Text = "Percorso di output";
            txtPercorsoOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPercorsoOutput
            // 
            btnPercorsoOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPercorsoOutput.BackColor = System.Drawing.Color.WhiteSmoke;
            btnPercorsoOutput.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnPercorsoOutput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnPercorsoOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnPercorsoOutput.Location = new System.Drawing.Point(595, 266);
            btnPercorsoOutput.Margin = new System.Windows.Forms.Padding(0);
            btnPercorsoOutput.Name = "btnPercorsoOutput";
            btnPercorsoOutput.Size = new System.Drawing.Size(40, 32);
            btnPercorsoOutput.TabIndex = 29;
            btnPercorsoOutput.Text = "...";
            btnPercorsoOutput.UseCompatibleTextRendering = true;
            btnPercorsoOutput.UseVisualStyleBackColor = false;
            btnPercorsoOutput.Click += btnPercorsoOutput_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = System.Drawing.Color.Transparent;
            label7.ForeColor = System.Drawing.Color.White;
            label7.Location = new System.Drawing.Point(15, 272);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(130, 20);
            label7.TabIndex = 30;
            label7.Text = "Percorso di output";
            // 
            // txtBaseClassView
            // 
            txtBaseClassView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtBaseClassView.Location = new System.Drawing.Point(180, 142);
            txtBaseClassView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtBaseClassView.Name = "txtBaseClassView";
            txtBaseClassView.Size = new System.Drawing.Size(267, 27);
            txtBaseClassView.TabIndex = 28;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = System.Drawing.Color.Transparent;
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(15, 145);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(119, 20);
            label6.TabIndex = 32;
            label6.Text = "Classe base viste";
            // 
            // txtBaseClass
            // 
            txtBaseClass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtBaseClass.Location = new System.Drawing.Point(180, 102);
            txtBaseClass.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtBaseClass.Name = "txtBaseClass";
            txtBaseClass.Size = new System.Drawing.Size(267, 27);
            txtBaseClass.TabIndex = 27;
            // 
            // BaseClass
            // 
            BaseClass.AutoSize = true;
            BaseClass.BackColor = System.Drawing.Color.Transparent;
            BaseClass.ForeColor = System.Drawing.Color.White;
            BaseClass.Location = new System.Drawing.Point(15, 106);
            BaseClass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            BaseClass.Name = "BaseClass";
            BaseClass.Size = new System.Drawing.Size(135, 20);
            BaseClass.TabIndex = 31;
            BaseClass.Text = "Classe base tabelle";
            // 
            // txtNameSpace
            // 
            txtNameSpace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtNameSpace.Location = new System.Drawing.Point(180, 62);
            txtNameSpace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtNameSpace.Name = "txtNameSpace";
            txtNameSpace.Size = new System.Drawing.Size(454, 27);
            txtNameSpace.TabIndex = 26;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = System.Drawing.Color.Transparent;
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(15, 65);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(89, 20);
            label5.TabIndex = 33;
            label5.Text = "NameSpace";
            // 
            // chkNumericiSpecifici
            // 
            chkNumericiSpecifici.AutoSize = true;
            chkNumericiSpecifici.ForeColor = System.Drawing.Color.White;
            chkNumericiSpecifici.Location = new System.Drawing.Point(304, 225);
            chkNumericiSpecifici.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chkNumericiSpecifici.Name = "chkNumericiSpecifici";
            chkNumericiSpecifici.Size = new System.Drawing.Size(255, 24);
            chkNumericiSpecifici.TabIndex = 39;
            chkNumericiSpecifici.Text = "Utilizza tipi dati numerici specifici";
            chkNumericiSpecifici.UseVisualStyleBackColor = true;
            chkNumericiSpecifici.Visible = false;
            // 
            // txtProgetto
            // 
            txtProgetto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtProgetto.Location = new System.Drawing.Point(180, 22);
            txtProgetto.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtProgetto.Name = "txtProgetto";
            txtProgetto.Size = new System.Drawing.Size(267, 27);
            txtProgetto.TabIndex = 40;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.BackColor = System.Drawing.Color.Transparent;
            lblName.ForeColor = System.Drawing.Color.White;
            lblName.Location = new System.Drawing.Point(15, 25);
            lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(67, 20);
            lblName.TabIndex = 41;
            lblName.Text = "Progetto";
            // 
            // ucPRJConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(txtProgetto);
            Controls.Add(lblName);
            Controls.Add(chkNumericiSpecifici);
            Controls.Add(label9);
            Controls.Add(txtdefaultvalue);
            Controls.Add(txtBaseClassProcedure);
            Controls.Add(label8);
            Controls.Add(txtPercorsoOutput);
            Controls.Add(btnPercorsoOutput);
            Controls.Add(label7);
            Controls.Add(txtBaseClassView);
            Controls.Add(label6);
            Controls.Add(txtBaseClass);
            Controls.Add(BaseClass);
            Controls.Add(txtNameSpace);
            Controls.Add(label5);
            Controls.Add(cmdCancel);
            Controls.Add(btnSave);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "ucPRJConfig";
            Size = new System.Drawing.Size(653, 374);
            Load += ucDBConfig_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtdefaultvalue;
        private System.Windows.Forms.TextBox txtBaseClassProcedure;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label txtPercorsoOutput;
        private System.Windows.Forms.Button btnPercorsoOutput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBaseClassView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBaseClass;
        private System.Windows.Forms.Label BaseClass;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkNumericiSpecifici;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtProgetto;
        private System.Windows.Forms.Label lblName;
    }
}
