using System.Drawing;
using System.Windows.Forms;

namespace Client.Views
{
    public partial class Profile : Form
    {
        public void LoadData()
        {

        }
        public Profile()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        private void editBtn_Click( object sender, System.EventArgs e )
        {
            panel1.Visible = true;
            panel1.Location = new Point( 12, 12 );
        }

        private void saveBtn_Click( object sender, System.EventArgs e )
        {
            panel1.Visible = false;
            panel1.Location = new Point( 12, 342 );

            //код для редактирования
        }
    }
}
