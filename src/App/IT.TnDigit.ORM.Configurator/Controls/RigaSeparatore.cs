using System.ComponentModel;
using System.Drawing;

namespace IT.InfoTn.Win.Controls
{
    public partial class RigaSeparatore : System.Windows.Forms.Panel
    {
        private Color colore = Color.Black;

        public Color Colore
        {
            get { return colore; }
            set
            {
                colore = value;
                Refresh();
            }
        }
        private int dimensione = 1;

        public int Dimensione
        {
            get { return dimensione; }
            set
            {
                dimensione = value;
                Refresh();
            }
        }


        public RigaSeparatore()
        {
            InitializeComponent();
        }

        public RigaSeparatore(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            this.Height = dimensione;
            Pen p = new Pen(colore, (float)dimensione);
            e.Graphics.DrawLine(p, new Point(0, 0), new Point(this.Width, 0));
        }
    }
}
