using System.Configuration;
using System.Windows.Forms;
using Client.Views;

namespace Client
{
    public partial class Main : Form
    {
        static private Form activeForm = null;

        public Main()
        {
            var config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );
            
            int startCount = int.Parse( config.AppSettings.Settings["startCount"].Value );
            startCount++;
            config.AppSettings.Settings["startCount"].Value = startCount.ToString();
            config.Save();

            InitializeComponent();
            openChildForm(new Katalog());

            if (startCount < 2)
            {
                Registration reg = new Registration();
                reg.Show();
            }
        }

        static public void openChildForm(Form childForm)
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
            openChildForm(new Katalog());
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            openChildForm(new Bascket());
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            openChildForm(new Katalog());
        }

        private void button3_Click( object sender, System.EventArgs e )
        {
            openChildForm( new Profile() );
        }
    }
}
