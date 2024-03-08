using System.Windows.Forms;

namespace Client
{
    public partial class Main : Form
    {
        private Form activeForm = null;

        public Main()
        {
            Settings.Default.startCount++;

            InitializeComponent();
            openChildForm(new Katalog());

            if (Settings.Default.startCount < 2)
            {
                Registration reg = new Registration();
                reg.Show();
            }
        }

        private void openChildForm(Form childForm)
        {
            if (activeForm != null) activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(childForm);
            mainPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void label1_Click(object sender, System.EventArgs e)
        {
            Katalog._Load(Katalog.products);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            openChildForm(new Bascket());
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            openChildForm(new Katalog());
        }
    }
}
