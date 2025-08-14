namespace IT.TnDigit.ORM.Configurator.Configurations
{
    partial class ucDBConfig
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
            btnChooseDB = new System.Windows.Forms.Button();
            pnlDatabaseParams = new System.Windows.Forms.Panel();
            txtPassword = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            txtUtente = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txtOwner = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            txtDatabase = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txtServer = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            cmbTipoDB = new IT.InfoTn.Win.Controls.FlatCombo(components);
            btnSave = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            pnlDatabaseParams.SuspendLayout();
            SuspendLayout();
            // 
            // btnChooseDB
            // 
            btnChooseDB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnChooseDB.BackColor = System.Drawing.Color.WhiteSmoke;
            btnChooseDB.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            btnChooseDB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnChooseDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnChooseDB.Location = new System.Drawing.Point(595, 40);
            btnChooseDB.Margin = new System.Windows.Forms.Padding(0);
            btnChooseDB.Name = "btnChooseDB";
            btnChooseDB.Size = new System.Drawing.Size(40, 32);
            btnChooseDB.TabIndex = 27;
            btnChooseDB.Text = "...";
            btnChooseDB.UseCompatibleTextRendering = true;
            btnChooseDB.UseVisualStyleBackColor = false;
            btnChooseDB.Click += btnChooseDB_Click;
            // 
            // pnlDatabaseParams
            // 
            pnlDatabaseParams.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlDatabaseParams.BackColor = System.Drawing.Color.Transparent;
            pnlDatabaseParams.Controls.Add(btnChooseDB);
            pnlDatabaseParams.Controls.Add(txtPassword);
            pnlDatabaseParams.Controls.Add(label4);
            pnlDatabaseParams.Controls.Add(txtUtente);
            pnlDatabaseParams.Controls.Add(label3);
            pnlDatabaseParams.Controls.Add(txtOwner);
            pnlDatabaseParams.Controls.Add(label10);
            pnlDatabaseParams.Controls.Add(txtDatabase);
            pnlDatabaseParams.Controls.Add(label2);
            pnlDatabaseParams.Controls.Add(txtServer);
            pnlDatabaseParams.Controls.Add(label1);
            pnlDatabaseParams.Enabled = false;
            pnlDatabaseParams.Location = new System.Drawing.Point(0, 52);
            pnlDatabaseParams.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnlDatabaseParams.Name = "pnlDatabaseParams";
            pnlDatabaseParams.Size = new System.Drawing.Size(649, 208);
            pnlDatabaseParams.TabIndex = 23;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPassword.Location = new System.Drawing.Point(119, 154);
            txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new System.Drawing.Size(514, 27);
            txtPassword.TabIndex = 20;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.ForeColor = System.Drawing.Color.White;
            label4.Location = new System.Drawing.Point(15, 158);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(70, 20);
            label4.TabIndex = 23;
            label4.Text = "Password";
            // 
            // txtUtente
            // 
            txtUtente.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUtente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtUtente.Location = new System.Drawing.Point(119, 117);
            txtUtente.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtUtente.Name = "txtUtente";
            txtUtente.Size = new System.Drawing.Size(514, 27);
            txtUtente.TabIndex = 19;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = System.Drawing.Color.Transparent;
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(15, 122);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(53, 20);
            label3.TabIndex = 26;
            label3.Text = "Utente";
            // 
            // txtOwner
            // 
            txtOwner.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtOwner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtOwner.Location = new System.Drawing.Point(119, 77);
            txtOwner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtOwner.Name = "txtOwner";
            txtOwner.Size = new System.Drawing.Size(514, 27);
            txtOwner.TabIndex = 18;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = System.Drawing.Color.Transparent;
            label10.ForeColor = System.Drawing.Color.White;
            label10.Location = new System.Drawing.Point(15, 80);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(52, 20);
            label10.TabIndex = 25;
            label10.Text = "Owner";
            // 
            // txtDatabase
            // 
            txtDatabase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtDatabase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDatabase.Location = new System.Drawing.Point(119, 40);
            txtDatabase.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new System.Drawing.Size(471, 27);
            txtDatabase.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(15, 43);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 20);
            label2.TabIndex = 24;
            label2.Text = "Database";
            // 
            // txtServer
            // 
            txtServer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtServer.Location = new System.Drawing.Point(119, 3);
            txtServer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtServer.Name = "txtServer";
            txtServer.Size = new System.Drawing.Size(514, 27);
            txtServer.TabIndex = 16;
            txtServer.Text = "ORA";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(15, 8);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(50, 20);
            label1.TabIndex = 22;
            label1.Text = "Server";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = System.Drawing.Color.Transparent;
            label11.ForeColor = System.Drawing.Color.White;
            label11.Location = new System.Drawing.Point(15, 23);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(39, 20);
            label11.TabIndex = 21;
            label11.Text = "Tipo";
            // 
            // cmbTipoDB
            // 
            cmbTipoDB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbTipoDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTipoDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cmbTipoDB.FormattingEnabled = true;
            cmbTipoDB.Items.AddRange(new object[] { "Oracle", "Microsoft Access", "Microsoft SQL Server", "Microsoft SQL Server File", "SQLLite", "PostgreSQL" });
            cmbTipoDB.Location = new System.Drawing.Point(119, 11);
            cmbTipoDB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cmbTipoDB.Name = "cmbTipoDB";
            cmbTipoDB.Size = new System.Drawing.Size(513, 28);
            cmbTipoDB.TabIndex = 22;
            cmbTipoDB.SelectedIndexChanged += cmbTipoDB_SelectedIndexChanged;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnSave.Location = new System.Drawing.Point(549, 266);
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
            cmdCancel.Location = new System.Drawing.Point(441, 266);
            cmdCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 35);
            cmdCancel.TabIndex = 25;
            cmdCancel.Text = "&ANNULLA";
            cmdCancel.UseVisualStyleBackColor = true;
            cmdCancel.Click += cmdCancel_Click;
            // 
            // ucDBConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(cmdCancel);
            Controls.Add(pnlDatabaseParams);
            Controls.Add(label11);
            Controls.Add(cmbTipoDB);
            Controls.Add(btnSave);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "ucDBConfig";
            Size = new System.Drawing.Size(653, 309);
            Load += ucDBConfig_Load;
            pnlDatabaseParams.ResumeLayout(false);
            pnlDatabaseParams.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChooseDB;
        private System.Windows.Forms.Panel pnlDatabaseParams;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUtente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOwner;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private IT.InfoTn.Win.Controls.FlatCombo cmbTipoDB;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button cmdCancel;
    }
}
