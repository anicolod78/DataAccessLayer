using System.ComponentModel;
using System.Drawing;

namespace IT.InfoTn.Win.Controls
{
    public partial class FlatCombo : System.Windows.Forms.ComboBox
    {
        public FlatCombo()
        {
            InitializeComponent();
        }

        public FlatCombo(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {

            base.OnPaint(e);

            Rectangle rc = new Rectangle(1, 1, this.Width - 1, this.Height - 1);
            Pen p = new Pen(Color.Black, 1);
            e.Graphics.DrawRectangle(p, rc);
        }
    }
}
