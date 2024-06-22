using System.Windows.Forms;

namespace Client.Views
{
    public partial class CardForm : Form
    {
        public CardForm(Product product)
        {
            InitializeComponent();

            nameLabel.Text = product.Name;
            descLabel.Text = product.Description;
            priceLabel.Text = product.Price;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Katalog.DecodeBase64Image( product.ImgBase64 );

            button1.Click += async ( sender, e ) => await Katalog.BuyButtonClick( product.Id );
        }
    }
}
