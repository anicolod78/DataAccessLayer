using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IT.InfoTn.Win.Controls
{
    public partial class PulsanteGrafico : System.Windows.Forms.Button
    {
        private Image backgroundImageHover = null;
        private Image backgroundImageNormal = null;

        private Color backgroundColorHover = Color.White;
        private Color backgroundColorNormal = SystemColors.ButtonFace;

        private Color borderColor = Color.Black;

        private bool usaImmagini = false;


        public PulsanteGrafico()
        {
            InitializeComponent();

            this.FlatStyle = FlatStyle.Flat;
            this.AutoSize = false;
            this.AutoEllipsis = true;
            this.Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: inserire qui il codice personalizzato

            // Chiamata della classe base OnPaint
            base.OnPaint(pe);


        }

        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
            }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (usaImmagini == true)
            {
                this.BackgroundImage = this.BackgroundImageHover;
            }
            else
            {
                this.BackColor = this.BackgroundColorHover;
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (usaImmagini == true)
            {
                this.BackgroundImage = this.BackgroundImageNormal;
            }
            else
            {
                this.BackColor = this.BackgroundColorNormal;
            }

            base.OnMouseLeave(e);
        }

        public bool UsaImmagini
        {
            get { return usaImmagini; }
            set { usaImmagini = value; }
        }


        public Image BackgroundImageNormal
        {
            get
            {
                return backgroundImageNormal;
            }
            set
            {
                this.BackgroundImage = value;
                backgroundImageNormal = value;
            }
        }

        public Image BackgroundImageHover
        {
            get
            {
                return backgroundImageHover;
            }
            set
            {
                backgroundImageHover = value;
            }
        }


        public Color BackgroundColorHover
        {
            get { return backgroundColorHover; }
            set { backgroundColorHover = value; }
        }

        public Color BackgroundColorNormal
        {
            get { return backgroundColorNormal; }
            set
            {
                this.BackColor = value;
                backgroundColorNormal = value;
            }
        }
    }
}
