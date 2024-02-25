using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Katalog : Form
    {
        public Katalog()
        {
            InitializeComponent();
        }

        private static async Task<string> SendRequest(string server, int port, string command)
        {
            IPAddress ipAddress = null;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(server);

            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily ==
                  AddressFamily.InterNetwork)
                {
                    ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }

            if (ipAddress == null)
                throw new Exception("No IPv4 address for server");

            TcpClient client = new TcpClient();
            await client.ConnectAsync(ipAddress, port);
            NetworkStream networkStream = client.GetStream();

            StreamWriter writer = new StreamWriter(networkStream);
            StreamReader reader = new StreamReader(networkStream);

            writer.AutoFlush = true;
            string requestData = "command=" + command + "&";
            await writer.WriteLineAsync(requestData);
            string response = await reader.ReadLineAsync();
            client.Close();
            return response;
        }

        private Image DecodeBase64Image(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }

        private async Task LoadProduct()
        {
            try
            {
                string server = "127.0.0.1";
                int port = 4444;
                string command = "LoadProductsPlease";
                string sResponse = await SendRequest(server, port, command);

                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(sResponse);
                
                foreach (Product product in products)
                {
                    CreateCard(new Point(10, 10), product);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //locations:
        //47; 10
        //341;10
        //635;10
        //step = 74
        public void CreateCard(Point location, Product product)
        {
            Panel cardPanel = new Panel();
            cardPanel.Location = location;
            cardPanel.Size = new Size(220, 355);
            cardPanel.BorderStyle = BorderStyle.FixedSingle;

            // Создание картинки товара
            PictureBox imageBox = new PictureBox();
            imageBox.Parent = cardPanel;
            imageBox.Location = new Point(0, 0);
            imageBox.Size = new Size(220, 220);
            imageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBox.Image = DecodeBase64Image(product.ImgBase64);

            // Создание названия товара
            Label nameLabel = new Label();
            nameLabel.Parent = cardPanel;
            nameLabel.Location = new Point(0, 226);
            nameLabel.Size = new Size(220, 48);
            nameLabel.AutoSize = false;
            nameLabel.AutoEllipsis = true;
            nameLabel.Font = new Font("Arial", 12);
            nameLabel.Text = product.Name;

            // Создание цены товара
            Label priceLabel = new Label();
            priceLabel.Parent = cardPanel;
            priceLabel.Location = new Point(0, 283);
            priceLabel.Size = new Size(220, 30);
            priceLabel.AutoSize = true;
            priceLabel.Font = new Font("Arial", 12);
            priceLabel.Text = product.Price + " ₽";

            // Создание кнопки добавления в корзину
            Button addButton = new Button();
            addButton.Parent = cardPanel;
            addButton.Location = new Point(60, 320);
            addButton.Size = new Size(100, 35);
            addButton.Font = new Font("Arial", 12, FontStyle.Bold);
            addButton.ForeColor = Color.White;
            addButton.BackColor = ColorTranslator.FromHtml("#e21235");
            addButton.Text = "В корзину";
            addButton.Click += (sender, e) => BuyButtonClick(product.Id);

            // Добавление элементов карточки на панель
            cardPanel.Controls.Add(imageBox);
            cardPanel.Controls.Add(nameLabel);
            cardPanel.Controls.Add(priceLabel);
            cardPanel.Controls.Add(addButton);

            // Добавление панели на форму
            productsPanel.Controls.Add(cardPanel);
        }

        private void BuyButtonClick(int productId)
        {
            MessageBox.Show($"okey {productId}");
            // Ваш код для обработки действия при нажатии кнопки "Купить" для выбранного товара
            // Используйте productId для получения нужного вам товара из базы данных и выполнения соответствующих действий
        }

        private async void Katalog_Load(object sender, EventArgs e)
        {
            await LoadProduct();
        }
    }
}
