using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
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

        int i = 1;
        int x = 47;
        int y = 10;

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

        private bool IsValidBase64String(string base64String)
        {
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Image DecodeBase64Image(string base64Image)
        {
            if (!IsValidBase64String(base64Image))
            {
                throw new ArgumentException("Недопустимый параметр. Неверное Base64-представление изображения.");
            }

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
                
                foreach(var product in products)
                {
                    if(i % 3 == 0)
                    {
                        y += 375;
                        x = 47;
                    }
                    CreateCard(new Point(x, y), product);
                    x += 294;
                    i++;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //panel size = 902; 552
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
            cardPanel.BorderStyle = BorderStyle.None;
            cardPanel.Margin = new Padding(60, 20, 0, 0);

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
            productPanel.Controls.Add(cardPanel);
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

        private void label1_Click(object sender, EventArgs e)
        {
            //смартфоны и гаджеты
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //Ноутбуки и компьютеры
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //Телевизоры и цифровое ТВ
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //Аудиотехника
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //Техника для кухни
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //Техника для дома
        }

        private void label7_Click(object sender, EventArgs e)
        {
            //Красота и здоровье
        }

        private void label8_Click(object sender, EventArgs e)
        {
            //Умный дом
        }

        private void label9_Click(object sender, EventArgs e)
        {
            //Игры и софт
        }

        private void label10_Click(object sender, EventArgs e)
        {
            //Premium
        }

        private void label11_Click(object sender, EventArgs e)
        {
            //Хобби и развлечения
        }

        private void label12_Click(object sender, EventArgs e)
        {
            //Спортивные товары
        }

        private void label13_Click(object sender, EventArgs e)
        {
            //Электроинструменты и садовая техника
        }

        private void label14_Click(object sender, EventArgs e)
        {
            //Товары для дома
        }

        private void label15_Click(object sender, EventArgs e)
        {
            //Фото и видео
        }

        private void label16_Click(object sender, EventArgs e)
        {
            //Автоэлектроника
        }

        private void label17_Click(object sender, EventArgs e)
        {
            //Аксессуары
        }
    }
}
