using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IT.InfoTn.Win.Controls
{
    /// <summary>
    /// Descrizione di riepilogo per UserControl1.
    /// </summary>
    public class GroupBox : System.Windows.Forms.GroupBox
    {
        private bool stiloso = true;

        private Color coloreBordoFieldset = Color.FromArgb(0, 0, 0);
        private Color coloreSfondoFieldset = Color.FromArgb(255, 255, 255);
        private Color coloreBordoLegend = Color.FromArgb(0, 0, 0);
        private Color coloreSfondoLegend = Color.FromArgb(255, 255, 255);
        private Color coloreTestoLegend = Color.FromArgb(255, 255, 255);
        private Font fontTestoLegend = null;
        private float raggio = 1;

        public float Raggio
        {
            get { return raggio; }
            set
            {
                if (value < 1)
                    raggio = 1;
                else
                    raggio = value;

                this.Refresh();
            }
        }

        public Color ColoreTestoLegend
        {
            get { return coloreTestoLegend; }
            set
            {
                coloreTestoLegend = value;
                this.Refresh();
            }
        }

        public Font FontTestoLegend
        {
            get
            {
                if (fontTestoLegend == null)
                    fontTestoLegend = this.Font;

                return fontTestoLegend;
            }
            set
            {
                fontTestoLegend = value;
                this.Refresh();
            }
        }

        public Color ColoreSfondoLegend
        {
            get { return coloreSfondoLegend; }
            set
            {
                coloreSfondoLegend = value;
                this.Refresh();
            }
        }

        public Color ColoreBordoLegend
        {
            get { return coloreBordoLegend; }
            set
            {
                coloreBordoLegend = value;
                this.Refresh();
            }
        }

        public Color ColoreSfondoFieldset
        {
            get { return coloreSfondoFieldset; }
            set
            {
                coloreSfondoFieldset = value;
                this.Refresh();
            }
        }

        public Color ColoreBordoFieldset
        {
            get { return coloreBordoFieldset; }
            set
            {
                coloreBordoFieldset = value;
                this.Refresh();
            }
        }



        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public GroupBox()
        {
            // Chiamata richiesta da Progettazione form Windows.Forms.
            InitializeComponent();

            // TODO: aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent.

        }

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [Description("Attiva la visualizzazione dei colori e dei bordi arrotondati"), DefaultValue(true)]
        public bool Stiloso
        {
            get { return this.stiloso; }
            set
            {
                this.stiloso = value;
                this.Refresh();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.Stiloso == true)
            {
                pevent.Graphics.Clear(this.BackColor);
                //float raggio = 10;
                float diametro = raggio * 2;
                float top = 0;
                if (this.Text == "")
                    top = raggio / 2;
                else
                    top = pevent.Graphics.MeasureString(this.Text, this.FontTestoLegend).Height / 2;

                float larg = this.Width - 1;
                float alt = this.Height - 1;

                //disegno lo sfondo
                System.Drawing.Pen pen = new Pen(this.coloreBordoFieldset);
                System.Drawing.Brush brush = new SolidBrush(this.coloreSfondoFieldset);
                System.Drawing.Drawing2D.GraphicsPath pth = new System.Drawing.Drawing2D.GraphicsPath();
                pth.AddArc(0, top, diametro, diametro, 180, 90);
                pth.AddLine(raggio, top, this.Width - raggio, top);
                pth.AddArc(larg - diametro, top, diametro, diametro, 270, 90);
                pth.AddLine(larg, top + raggio, larg, alt - top - (raggio / 2));
                pth.AddArc(larg - diametro, alt - diametro, diametro, diametro, 0, 90);
                pth.AddLine(raggio, alt, larg - raggio, alt);
                pth.AddArc(0, alt - diametro, diametro, diametro, 90, 90);
                pth.AddLine(0, top + raggio, 0, alt - top - (raggio / 2));
                pevent.Graphics.FillPath(brush, pth);
                pevent.Graphics.DrawPath(pen, pth);

                if (this.Text != "")
                {
                    //disegno il testo
                    pen = new Pen(this.coloreBordoLegend);
                    brush = new SolidBrush(this.coloreSfondoLegend);
                    pth = new System.Drawing.Drawing2D.GraphicsPath();

                    float left = 10;
                    alt = pevent.Graphics.MeasureString(this.Text, this.FontTestoLegend).Height + 2;
                    larg = pevent.Graphics.MeasureString(this.Text, this.FontTestoLegend).Width;
                    pth.AddArc(left, 0, diametro, diametro, 180, 90);
                    pth.AddLine(left + raggio + larg, 0, larg, 0);
                    pth.AddArc(larg + left, 0, diametro, diametro, 270, 90);
                    pth.AddLine(larg + left + diametro, raggio, larg + left + diametro, alt - raggio);
                    pth.AddArc(larg + left, alt - diametro, diametro, diametro, 0, 90);
                    pth.AddLine(left + raggio, alt, larg - raggio, alt);
                    pth.AddArc(left, alt - diametro, diametro, diametro, 90, 90);
                    pth.AddLine(left, raggio, left, alt - (raggio / 2));
                    pevent.Graphics.FillPath(brush, pth);
                    pevent.Graphics.DrawPath(pen, pth);

                    pevent.Graphics.DrawString(this.Text, this.FontTestoLegend, new SolidBrush(this.coloreTestoLegend), left + raggio, 1);
                }



            }
            else
            {
                base.OnPaintBackground(pevent);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Stiloso == false)
            {
                base.OnPaint(e);
            }

            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(System.Windows.Forms.Label))
                    ((Label)ctl).BackColor = Color.Transparent;

                if (ctl.GetType() == typeof(IT.InfoTn.Win.Controls.GroupBox))
                    if (((IT.InfoTn.Win.Controls.GroupBox)ctl).stiloso == true)
                        ((IT.InfoTn.Win.Controls.GroupBox)ctl).BackColor = this.coloreSfondoFieldset;
            }
        }

        #region Codice generato da Progettazione componenti
        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
