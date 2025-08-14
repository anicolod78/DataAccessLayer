namespace IT.TnDigit.ORM.Configurator
{
    partial class frmMain
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

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnNewProjectConfig = new System.Windows.Forms.ToolStripButton();
            btnOpenProjectConfig = new System.Windows.Forms.ToolStripButton();
            btnSaveProjectConfig = new System.Windows.Forms.ToolStripButton();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            label1 = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            pnlDB = new IT.InfoTn.Win.Controls.GroupBox();
            lblDBStatus = new System.Windows.Forms.Label();
            lblDBName = new System.Windows.Forms.Label();
            lblDBType = new System.Windows.Forms.Label();
            lnkDBConfiguration = new System.Windows.Forms.LinkLabel();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            pnlRUL = new IT.InfoTn.Win.Controls.GroupBox();
            lnkRULCreation = new System.Windows.Forms.LinkLabel();
            pictureBox4 = new System.Windows.Forms.PictureBox();
            pnlPRJ = new IT.InfoTn.Win.Controls.GroupBox();
            lblPRJNamespace = new System.Windows.Forms.Label();
            lblPRJName = new System.Windows.Forms.Label();
            lnkPRJConfiguration = new System.Windows.Forms.LinkLabel();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            pnlOBJ = new IT.InfoTn.Win.Controls.GroupBox();
            lblOBJFunctions = new System.Windows.Forms.Label();
            lblOBJViews = new System.Windows.Forms.Label();
            lblOBJTables = new System.Windows.Forms.Label();
            lnkOBJSelection = new System.Windows.Forms.LinkLabel();
            pictureBox3 = new System.Windows.Forms.PictureBox();
            btnStartGeneration = new IT.InfoTn.Win.Controls.PulsanteGrafico();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlRUL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            pnlPRJ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            pnlOBJ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.White;
            toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnNewProjectConfig, btnOpenProjectConfig, btnSaveProjectConfig });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            toolStrip1.Size = new System.Drawing.Size(993, 39);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnNewProjectConfig
            // 
            btnNewProjectConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnNewProjectConfig.Image = Properties.Resources._new;
            btnNewProjectConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnNewProjectConfig.Name = "btnNewProjectConfig";
            btnNewProjectConfig.Size = new System.Drawing.Size(36, 36);
            btnNewProjectConfig.Text = "toolStripButton1";
            btnNewProjectConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            btnNewProjectConfig.Click += btnNewProjectConfig_Click;
            // 
            // btnOpenProjectConfig
            // 
            btnOpenProjectConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnOpenProjectConfig.Image = Properties.Resources.open;
            btnOpenProjectConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnOpenProjectConfig.Name = "btnOpenProjectConfig";
            btnOpenProjectConfig.Size = new System.Drawing.Size(36, 36);
            btnOpenProjectConfig.Text = "toolStripButton2";
            btnOpenProjectConfig.Click += btnOpenProjectConfig_Click;
            // 
            // btnSaveProjectConfig
            // 
            btnSaveProjectConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnSaveProjectConfig.Image = Properties.Resources.save;
            btnSaveProjectConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnSaveProjectConfig.Name = "btnSaveProjectConfig";
            btnSaveProjectConfig.Size = new System.Drawing.Size(36, 36);
            btnSaveProjectConfig.Text = "toolStripButton1";
            btnSaveProjectConfig.Click += btnSaveProjectConfig_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.Color.White;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new System.Drawing.Point(0, 39);
            splitContainer1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.DimGray;
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(lblTitle);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(993, 546);
            splitContainer1.SplitterDistance = 203;
            splitContainer1.SplitterWidth = 1;
            splitContainer1.TabIndex = 17;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(-1, 69);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            label1.Size = new System.Drawing.Size(204, 430);
            label1.TabIndex = 21;
            label1.Text = resources.GetString("label1.Text");
            label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(5, 1);
            lblTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            lblTitle.Size = new System.Drawing.Size(193, 68);
            lblTitle.TabIndex = 20;
            lblTitle.Text = "IT.TnDigit.ORM \r\nData types generator";
            lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = System.Drawing.Color.White;
            splitContainer2.Panel2.Controls.Add(btnStartGeneration);
            splitContainer2.Size = new System.Drawing.Size(789, 546);
            splitContainer2.SplitterDistance = 417;
            splitContainer2.SplitterWidth = 7;
            splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(pnlDB, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlRUL, 1, 1);
            tableLayoutPanel1.Controls.Add(pnlPRJ, 1, 0);
            tableLayoutPanel1.Controls.Add(pnlOBJ, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(14, 16, 14, 16);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(14, 16, 14, 16);
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(789, 417);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // pnlDB
            // 
            pnlDB.ColoreBordoFieldset = System.Drawing.Color.White;
            pnlDB.ColoreBordoLegend = System.Drawing.Color.Transparent;
            pnlDB.ColoreSfondoFieldset = System.Drawing.Color.SteelBlue;
            pnlDB.ColoreSfondoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlDB.ColoreTestoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlDB.Controls.Add(lblDBStatus);
            pnlDB.Controls.Add(lblDBName);
            pnlDB.Controls.Add(lblDBType);
            pnlDB.Controls.Add(lnkDBConfiguration);
            pnlDB.Controls.Add(pictureBox1);
            pnlDB.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlDB.FontTestoLegend = new System.Drawing.Font("Segoe UI", 9F);
            pnlDB.Location = new System.Drawing.Point(19, 20);
            pnlDB.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlDB.Name = "pnlDB";
            pnlDB.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlDB.Raggio = 10F;
            pnlDB.Size = new System.Drawing.Size(370, 184);
            pnlDB.TabIndex = 2;
            pnlDB.TabStop = false;
            // 
            // lblDBStatus
            // 
            lblDBStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblDBStatus.AutoSize = true;
            lblDBStatus.BackColor = System.Drawing.Color.Transparent;
            lblDBStatus.ForeColor = System.Drawing.Color.White;
            lblDBStatus.Location = new System.Drawing.Point(129, 99);
            lblDBStatus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblDBStatus.Name = "lblDBStatus";
            lblDBStatus.Size = new System.Drawing.Size(12, 20);
            lblDBStatus.TabIndex = 5;
            lblDBStatus.Text = ".";
            // 
            // lblDBName
            // 
            lblDBName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblDBName.AutoSize = true;
            lblDBName.BackColor = System.Drawing.Color.Transparent;
            lblDBName.ForeColor = System.Drawing.Color.White;
            lblDBName.Location = new System.Drawing.Point(129, 29);
            lblDBName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblDBName.Name = "lblDBName";
            lblDBName.Size = new System.Drawing.Size(12, 20);
            lblDBName.TabIndex = 6;
            lblDBName.Text = ".";
            // 
            // lblDBType
            // 
            lblDBType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblDBType.AutoSize = true;
            lblDBType.BackColor = System.Drawing.Color.Transparent;
            lblDBType.ForeColor = System.Drawing.Color.White;
            lblDBType.Location = new System.Drawing.Point(129, 63);
            lblDBType.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblDBType.Name = "lblDBType";
            lblDBType.Size = new System.Drawing.Size(12, 20);
            lblDBType.TabIndex = 7;
            lblDBType.Text = ".";
            // 
            // lnkDBConfiguration
            // 
            lnkDBConfiguration.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lnkDBConfiguration.AutoSize = true;
            lnkDBConfiguration.BackColor = System.Drawing.Color.Transparent;
            lnkDBConfiguration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lnkDBConfiguration.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            lnkDBConfiguration.LinkColor = System.Drawing.Color.White;
            lnkDBConfiguration.Location = new System.Drawing.Point(21, 143);
            lnkDBConfiguration.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lnkDBConfiguration.Name = "lnkDBConfiguration";
            lnkDBConfiguration.Size = new System.Drawing.Size(122, 20);
            lnkDBConfiguration.TabIndex = 4;
            lnkDBConfiguration.TabStop = true;
            lnkDBConfiguration.Text = "Database config";
            lnkDBConfiguration.LinkClicked += lnkDBConfiguration_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox1.Image = Properties.Resources.database;
            pictureBox1.Location = new System.Drawing.Point(24, 29);
            pictureBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(86, 89);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // pnlRUL
            // 
            pnlRUL.ColoreBordoFieldset = System.Drawing.Color.White;
            pnlRUL.ColoreBordoLegend = System.Drawing.Color.Transparent;
            pnlRUL.ColoreSfondoFieldset = System.Drawing.Color.Orange;
            pnlRUL.ColoreSfondoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlRUL.ColoreTestoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlRUL.Controls.Add(lnkRULCreation);
            pnlRUL.Controls.Add(pictureBox4);
            pnlRUL.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRUL.FontTestoLegend = new System.Drawing.Font("Segoe UI", 9F);
            pnlRUL.Location = new System.Drawing.Point(399, 212);
            pnlRUL.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlRUL.Name = "pnlRUL";
            pnlRUL.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlRUL.Raggio = 10F;
            pnlRUL.Size = new System.Drawing.Size(371, 185);
            pnlRUL.TabIndex = 10;
            pnlRUL.TabStop = false;
            // 
            // lnkRULCreation
            // 
            lnkRULCreation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lnkRULCreation.AutoSize = true;
            lnkRULCreation.BackColor = System.Drawing.Color.Transparent;
            lnkRULCreation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lnkRULCreation.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            lnkRULCreation.LinkColor = System.Drawing.Color.White;
            lnkRULCreation.Location = new System.Drawing.Point(21, 143);
            lnkRULCreation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lnkRULCreation.Name = "lnkRULCreation";
            lnkRULCreation.Size = new System.Drawing.Size(106, 20);
            lnkRULCreation.TabIndex = 4;
            lnkRULCreation.TabStop = true;
            lnkRULCreation.Text = "Creation rules";
            lnkRULCreation.LinkClicked += lnkRULCreation_LinkClicked;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox4.Image = Properties.Resources.configuration;
            pictureBox4.Location = new System.Drawing.Point(24, 29);
            pictureBox4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new System.Drawing.Size(86, 89);
            pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            // 
            // pnlPRJ
            // 
            pnlPRJ.ColoreBordoFieldset = System.Drawing.Color.White;
            pnlPRJ.ColoreBordoLegend = System.Drawing.Color.Transparent;
            pnlPRJ.ColoreSfondoFieldset = System.Drawing.Color.Firebrick;
            pnlPRJ.ColoreSfondoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlPRJ.ColoreTestoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlPRJ.Controls.Add(lblPRJNamespace);
            pnlPRJ.Controls.Add(lblPRJName);
            pnlPRJ.Controls.Add(lnkPRJConfiguration);
            pnlPRJ.Controls.Add(pictureBox2);
            pnlPRJ.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlPRJ.FontTestoLegend = new System.Drawing.Font("Segoe UI", 9F);
            pnlPRJ.Location = new System.Drawing.Point(399, 20);
            pnlPRJ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlPRJ.Name = "pnlPRJ";
            pnlPRJ.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlPRJ.Raggio = 10F;
            pnlPRJ.Size = new System.Drawing.Size(371, 184);
            pnlPRJ.TabIndex = 9;
            pnlPRJ.TabStop = false;
            // 
            // lblPRJNamespace
            // 
            lblPRJNamespace.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblPRJNamespace.AutoSize = true;
            lblPRJNamespace.BackColor = System.Drawing.Color.Transparent;
            lblPRJNamespace.ForeColor = System.Drawing.Color.White;
            lblPRJNamespace.Location = new System.Drawing.Point(129, 63);
            lblPRJNamespace.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblPRJNamespace.Name = "lblPRJNamespace";
            lblPRJNamespace.Size = new System.Drawing.Size(12, 20);
            lblPRJNamespace.TabIndex = 6;
            lblPRJNamespace.Text = ".";
            // 
            // lblPRJName
            // 
            lblPRJName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblPRJName.AutoSize = true;
            lblPRJName.BackColor = System.Drawing.Color.Transparent;
            lblPRJName.ForeColor = System.Drawing.Color.White;
            lblPRJName.Location = new System.Drawing.Point(129, 29);
            lblPRJName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblPRJName.Name = "lblPRJName";
            lblPRJName.Size = new System.Drawing.Size(12, 20);
            lblPRJName.TabIndex = 7;
            lblPRJName.Text = ".";
            // 
            // lnkPRJConfiguration
            // 
            lnkPRJConfiguration.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lnkPRJConfiguration.AutoSize = true;
            lnkPRJConfiguration.BackColor = System.Drawing.Color.Transparent;
            lnkPRJConfiguration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lnkPRJConfiguration.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            lnkPRJConfiguration.LinkColor = System.Drawing.Color.White;
            lnkPRJConfiguration.Location = new System.Drawing.Point(21, 143);
            lnkPRJConfiguration.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lnkPRJConfiguration.Name = "lnkPRJConfiguration";
            lnkPRJConfiguration.Size = new System.Drawing.Size(106, 20);
            lnkPRJConfiguration.TabIndex = 4;
            lnkPRJConfiguration.TabStop = true;
            lnkPRJConfiguration.Text = "Project config";
            lnkPRJConfiguration.LinkClicked += lnkPRJConfiguration_LinkClicked;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox2.Image = Properties.Resources.project;
            pictureBox2.Location = new System.Drawing.Point(24, 29);
            pictureBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(86, 89);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // pnlOBJ
            // 
            pnlOBJ.ColoreBordoFieldset = System.Drawing.Color.White;
            pnlOBJ.ColoreBordoLegend = System.Drawing.Color.Transparent;
            pnlOBJ.ColoreSfondoFieldset = System.Drawing.Color.DarkOrchid;
            pnlOBJ.ColoreSfondoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlOBJ.ColoreTestoLegend = System.Drawing.Color.FromArgb(255, 255, 255);
            pnlOBJ.Controls.Add(lblOBJFunctions);
            pnlOBJ.Controls.Add(lblOBJViews);
            pnlOBJ.Controls.Add(lblOBJTables);
            pnlOBJ.Controls.Add(lnkOBJSelection);
            pnlOBJ.Controls.Add(pictureBox3);
            pnlOBJ.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlOBJ.FontTestoLegend = new System.Drawing.Font("Segoe UI", 9F);
            pnlOBJ.Location = new System.Drawing.Point(19, 212);
            pnlOBJ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlOBJ.Name = "pnlOBJ";
            pnlOBJ.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pnlOBJ.Raggio = 10F;
            pnlOBJ.Size = new System.Drawing.Size(370, 185);
            pnlOBJ.TabIndex = 9;
            pnlOBJ.TabStop = false;
            // 
            // lblOBJFunctions
            // 
            lblOBJFunctions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblOBJFunctions.AutoSize = true;
            lblOBJFunctions.BackColor = System.Drawing.Color.Transparent;
            lblOBJFunctions.ForeColor = System.Drawing.Color.White;
            lblOBJFunctions.Location = new System.Drawing.Point(129, 99);
            lblOBJFunctions.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblOBJFunctions.Name = "lblOBJFunctions";
            lblOBJFunctions.Size = new System.Drawing.Size(12, 20);
            lblOBJFunctions.TabIndex = 6;
            lblOBJFunctions.Text = ".";
            // 
            // lblOBJViews
            // 
            lblOBJViews.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblOBJViews.AutoSize = true;
            lblOBJViews.BackColor = System.Drawing.Color.Transparent;
            lblOBJViews.ForeColor = System.Drawing.Color.White;
            lblOBJViews.Location = new System.Drawing.Point(129, 63);
            lblOBJViews.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblOBJViews.Name = "lblOBJViews";
            lblOBJViews.Size = new System.Drawing.Size(12, 20);
            lblOBJViews.TabIndex = 6;
            lblOBJViews.Text = ".";
            // 
            // lblOBJTables
            // 
            lblOBJTables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblOBJTables.AutoSize = true;
            lblOBJTables.BackColor = System.Drawing.Color.Transparent;
            lblOBJTables.ForeColor = System.Drawing.Color.White;
            lblOBJTables.Location = new System.Drawing.Point(130, 29);
            lblOBJTables.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblOBJTables.Name = "lblOBJTables";
            lblOBJTables.Size = new System.Drawing.Size(12, 20);
            lblOBJTables.TabIndex = 6;
            lblOBJTables.Text = ".";
            // 
            // lnkOBJSelection
            // 
            lnkOBJSelection.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lnkOBJSelection.AutoSize = true;
            lnkOBJSelection.BackColor = System.Drawing.Color.Transparent;
            lnkOBJSelection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lnkOBJSelection.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            lnkOBJSelection.LinkColor = System.Drawing.Color.White;
            lnkOBJSelection.Location = new System.Drawing.Point(21, 143);
            lnkOBJSelection.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lnkOBJSelection.Name = "lnkOBJSelection";
            lnkOBJSelection.Size = new System.Drawing.Size(127, 20);
            lnkOBJSelection.TabIndex = 4;
            lnkOBJSelection.TabStop = true;
            lnkOBJSelection.Text = "Objects selection";
            lnkOBJSelection.LinkClicked += lnkOBJSelection_LinkClicked;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = System.Drawing.Color.Transparent;
            pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox3.Image = Properties.Resources.objects;
            pictureBox3.Location = new System.Drawing.Point(24, 29);
            pictureBox3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new System.Drawing.Size(86, 89);
            pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // btnStartGeneration
            // 
            btnStartGeneration.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            btnStartGeneration.AutoEllipsis = true;
            btnStartGeneration.BackColor = System.Drawing.Color.CornflowerBlue;
            btnStartGeneration.BackgroundColorHover = System.Drawing.Color.LightSteelBlue;
            btnStartGeneration.BackgroundColorNormal = System.Drawing.Color.CornflowerBlue;
            btnStartGeneration.BackgroundImageHover = null;
            btnStartGeneration.BackgroundImageNormal = null;
            btnStartGeneration.BorderColor = System.Drawing.Color.Black;
            btnStartGeneration.Cursor = System.Windows.Forms.Cursors.Hand;
            btnStartGeneration.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            btnStartGeneration.FlatAppearance.BorderSize = 2;
            btnStartGeneration.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            btnStartGeneration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            btnStartGeneration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStartGeneration.Location = new System.Drawing.Point(331, 14);
            btnStartGeneration.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            btnStartGeneration.MinimumSize = new System.Drawing.Size(200, 0);
            btnStartGeneration.Name = "btnStartGeneration";
            btnStartGeneration.Size = new System.Drawing.Size(200, 67);
            btnStartGeneration.TabIndex = 0;
            btnStartGeneration.Text = "Avvia";
            btnStartGeneration.UsaImmagini = false;
            btnStartGeneration.UseVisualStyleBackColor = false;
            btnStartGeneration.Click += btnStartGeneration_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(993, 585);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            MinimumSize = new System.Drawing.Size(816, 595);
            Name = "frmMain";
            Text = "Data Access Driven Project Configuration";
            Load += frmMain_Load;
            SizeChanged += frmMain_SizeChanged;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            pnlDB.ResumeLayout(false);
            pnlDB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlRUL.ResumeLayout(false);
            pnlRUL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            pnlPRJ.ResumeLayout(false);
            pnlPRJ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            pnlOBJ.ResumeLayout(false);
            pnlOBJ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNewProjectConfig;
        private System.Windows.Forms.ToolStripButton btnOpenProjectConfig;
        private System.Windows.Forms.ToolStripButton btnSaveProjectConfig;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private IT.InfoTn.Win.Controls.GroupBox pnlRUL;
        private System.Windows.Forms.LinkLabel lnkRULCreation;
        private System.Windows.Forms.PictureBox pictureBox4;
        private IT.InfoTn.Win.Controls.GroupBox pnlOBJ;
        private System.Windows.Forms.LinkLabel lnkOBJSelection;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label lblTitle;
        private IT.InfoTn.Win.Controls.PulsanteGrafico btnStartGeneration;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private IT.InfoTn.Win.Controls.GroupBox pnlDB;
        private System.Windows.Forms.Label lblDBStatus;
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.Label lblDBType;
        private System.Windows.Forms.LinkLabel lnkDBConfiguration;
        private System.Windows.Forms.PictureBox pictureBox1;
        private IT.InfoTn.Win.Controls.GroupBox pnlPRJ;
        private System.Windows.Forms.Label lblPRJNamespace;
        private System.Windows.Forms.Label lblPRJName;
        private System.Windows.Forms.LinkLabel lnkPRJConfiguration;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblOBJFunctions;
        private System.Windows.Forms.Label lblOBJViews;
        private System.Windows.Forms.Label lblOBJTables;
        private System.Windows.Forms.Label label1;
    }
}

