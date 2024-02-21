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
            await client.ConnectAsync(ipAddress, port); // соединение
            NetworkStream networkStream = client.GetStream();
            StreamWriter writer = new StreamWriter(networkStream);
            StreamReader reader = new StreamReader(networkStream);
            writer.AutoFlush = true;
            string requestData = "command=" + command + "&"; // 'End-of-request'
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
                    /*
                    Panel panelProductCard = new Panel();
                    panelProductCard.BorderStyle = BorderStyle.FixedSingle;

                    PictureBox pictureBoxProduct = new PictureBox();
                    pictureBoxProduct.Image = DecodeBase64Image(product.ImgBase64);
                    pictureBoxProduct.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxProduct.Dock = DockStyle.Top;
                    panelProductCard.Controls.Add(pictureBoxProduct);

                    Label labelProductName = new Label();
                    labelProductName.Text = product.Name;
                    labelProductName.Dock = DockStyle.Top;
                    panelProductCard.Controls.Add(labelProductName);

                    Label labelProductPrice = new Label();
                    labelProductPrice.Text = product.Price.ToString();
                    labelProductPrice.Dock = DockStyle.Top;
                    panelProductCard.Controls.Add(labelProductPrice);

                    Button buttonBuy = new Button();
                    buttonBuy.Text = "Купить";
                    buttonBuy.Dock = DockStyle.Top;
                    buttonBuy.Click += (sender, e) => BuyButtonClick(product.Id);
                    panelProductCard.Controls.Add(buttonBuy);

                    this.Controls.Add(panelProductCard);
                    */
                    CreateCard(new Point(72, 87), product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CreateCard(Point location, Product product)
        {
            Panel cardPanel = new Panel();
            cardPanel.Location = location;
            cardPanel.Size = new Size(300, 400);
            cardPanel.BorderStyle = BorderStyle.FixedSingle;

            // Создание картинки товара
            PictureBox imageBox = new PictureBox();
            imageBox.Parent = cardPanel;
            imageBox.Location = new Point(3, 3);
            imageBox.Size = new Size(294, 200);
            imageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBox.Image = DecodeBase64Image(product.ImgBase64);

            // Создание названия товара
            Label nameLabel = new Label();
            nameLabel.Parent = cardPanel;
            nameLabel.Location = new Point(10, 220);
            nameLabel.Size = new Size(200, 100);
            nameLabel.AutoSize = true;
            nameLabel.Font = new Font("Arial", 12);
            nameLabel.Text = product.Name;

            // Создание цены товара
            Label priceLabel = new Label();
            priceLabel.Parent = cardPanel;
            priceLabel.Location = new Point(10, 290);
            priceLabel.Size = new Size(200, 25);
            priceLabel.AutoSize = true;
            priceLabel.Font = new Font("Arial", 12);
            priceLabel.Text = product.Price + " ₽";

            // Создание кнопки добавления в корзину
            Button addButton = new Button();
            addButton.Parent = cardPanel;
            addButton.Location = new Point(50, 340);
            addButton.Size = new Size(200, 50);
            addButton.Font = new Font("Arial", 12);
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
            this.Controls.Add(cardPanel);
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
