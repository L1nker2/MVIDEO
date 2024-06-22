using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;
using Client.Views;

namespace Client
{
    public partial class Katalog : Form
    {
        public Katalog()
        {
            InitializeComponent();
        }

        public static int i = 1;
        public static int x = 47;
        public static int y = 10;
        public static List<Product> products;

        static public async Task PerformSearchAsync(string searchText)
        {
            List<Product> searchResults = await Task.Run( () =>
            {
                // Поиск товаров, соответствующих введенному тексту (без учета регистра)
                return products.Where( p => p.Name.IndexOf( searchText, StringComparison.CurrentCultureIgnoreCase ) >= 0 ).ToList();
            } );

            // Отрисовка карточек товаров (например, вызов вашей функции отрисовки)
            await _Load(searchResults);
        }

        async static public Task TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = Main.textBoxSearch.Text;

            // Вызов асинхронного метода поиска при изменении текста
            await PerformSearchAsync(searchText);
        }

        public static async Task<string> SendRequest(string server, int port, string command)
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

        private static bool IsValidBase64String(string base64String)
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

        public static Image DecodeBase64Image(string base64Image)
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

        public async Task LoadProduct()
        {
            try
            {
                string server = "127.0.0.1";
                int port = 4444;
                string command = "LoadProductsPlease";
                string sResponse = await SendRequest(server, port, command);

                products = JsonConvert.DeserializeObject<List<Product>>(sResponse);
                
                foreach(var product in products)
                {
                    if(i % 3 == 0)
                    {
                        y += 375;
                        x = 47;
                    }
                    await CreateCard(new Point(x, y), product);
                    x += 294;   
                    i++;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        public static async Task CreateCard(Point location, Product product)
        {
            Panel cardPanel = new Panel();
            cardPanel.Location = location;
            cardPanel.Size = new Size( 220, 355 );
            cardPanel.BorderStyle = BorderStyle.None;
            cardPanel.Margin = new Padding( 60, 20, 0, 0 );

            // Создание картинки товара
            PictureBox imageBox = new PictureBox();
            imageBox.Parent = cardPanel;
            imageBox.Location = new Point( 0, 0 );
            imageBox.Size = new Size( 220, 220 );
            imageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBox.Image = DecodeBase64Image( product.ImgBase64 );
            imageBox.Cursor = Cursors.Hand;
            imageBox.Click += async ( sender, e ) => await ShowProduct( product.Id );

            // Создание названия товара
            Label nameLabel = new Label();
            nameLabel.Parent = cardPanel;
            nameLabel.Location = new Point( 0, 226 );
            nameLabel.Size = new Size( 220, 48 );
            nameLabel.AutoSize = false;
            nameLabel.AutoEllipsis = true;
            nameLabel.Font = new Font( "Arial", 12 );
            nameLabel.Text = product.Name;

            // Создание цены товара
            Label priceLabel = new Label();
            priceLabel.Parent = cardPanel;
            priceLabel.Location = new Point( 0, 283 );
            priceLabel.Size = new Size( 220, 30 );
            priceLabel.AutoSize = true;
            priceLabel.Font = new Font( "Arial", 12 );
            priceLabel.Text = product.Price + " ₽";

            // Создание кнопки добавления в корзину
            Button addButton = new Button();
            addButton.Parent = cardPanel;
            addButton.Location = new Point( 60, 320 );
            addButton.Size = new Size( 100, 35 );
            addButton.Font = new Font( "Arial", 12, FontStyle.Bold );
            addButton.ForeColor = Color.White;
            addButton.BackColor = ColorTranslator.FromHtml( "#e21235" );
            addButton.Text = "В корзину";
            addButton.Click += async ( sender, e ) => await BuyButtonClick( product.Id );

            // Добавление элементов карточки на панель
            cardPanel.Controls.Add( imageBox );
            cardPanel.Controls.Add( nameLabel );
            cardPanel.Controls.Add( priceLabel );
            cardPanel.Controls.Add( addButton );

            if (product.Count == 0)
            {
                cardPanel.Enabled = false;
            }

            // Добавление панели на форму
            productPanel.Controls.Add( cardPanel );
        }

        private static async Task ShowProduct(int productId )
        {
            Product product = new Product();
            for(i = 0; i < products.Count; i++)
            {
                if (products[i].Id == productId)
                {
                    product = products[i]; break;
                }
            }
            Main.openChildForm( new CardForm(product) );
        }

        public static async Task BuyButtonClick(int productId)
        {
            if (ConfigurationManager.AppSettings["isLogin"] == "false")
            {
                MessageBox.Show( "Чтобы добавить товар в корзину, необходимо войти/зарегестрироваться" );
                return;
            }
            try
            {
                string server = "127.0.0.1";
                int port = 4444;
                string command = $"AddToBasketPlease&userId={ConfigurationManager.AppSettings["userId"]}&productId={productId}";
                string sResponse = await SendRequest(server, port, command);

                if (sResponse != "Okey")
                {
                    MessageBox.Show($"На сервере произошла ошибка:\n{sResponse}");
                    return;
                }
                else
                {
                    notifyIcon1.Icon = SystemIcons.Information;
                    notifyIcon1.BalloonTipTitle = "Товар добавлен";
                    notifyIcon1.BalloonTipText = $"Товар успешно добавлен в корзину.";
                    notifyIcon1.ShowBalloonTip(1000); // Показать уведомление на 2 секунды
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async private void SelectCategory(string category)
        {
            productPanel.Controls.Clear();
            List<Product> productsWithCategory = new List<Product>();
            foreach(var product in products)
            {
                if(product.Category == category)
                {
                    productsWithCategory.Add(product);
                }
            }
            await _Load(productsWithCategory);
        }
        async public static Task _Load(List<Product> products)
        {
            if(products.Count == 0)
            {
                productPanel.Controls.Clear();
                Label label = new Label();
                label.AutoSize = true;
                label.Font = new Font("Arial", 20);
                label.Text = "Ничего не найдено";
                label.Parent = productPanel;
                label.ForeColor = Color.Red;
                productPanel.Controls.Add(label);
                return;
            }

            productPanel.Controls.Clear();
            foreach (var product in products)
            {
                if (i % 3 == 0)
                {
                    y += 375;
                    x = 47;
                }
                await CreateCard(new Point(x, y), product);
                x += 294;
                i++;
            }
        }

        private async void Katalog_Load(object sender, EventArgs e)
        {
            await LoadProduct();
        }

        #region labels
        private void label1_Click(object sender, EventArgs e)
        {
            //Смартфоны и гаджеты
            SelectCategory(label1.Text);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //Ноутбуки и компьютеры
            SelectCategory(label2.Text);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //Телевизоры и цифровое ТВ
            SelectCategory(label3.Text);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //Аудиотехника
            SelectCategory(label4.Text);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //Техника для кухни
            SelectCategory(label5.Text);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //Техника для дома
            SelectCategory(label6.Text);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            //Красота и здоровье
            SelectCategory(label7.Text);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            //Умный дом
            SelectCategory(label8.Text);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            //Игры и софт
            SelectCategory(label9.Text);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            //Хобби и развлечения
            SelectCategory(label11.Text);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            //Спортивные товары
            SelectCategory(label13.Text);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            //Электроинструменты и садовая техника
            SelectCategory(label13.Text);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            //Товары для дома
            SelectCategory(label14.Text);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            //Фото и видео
            SelectCategory(label15.Text);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            //Автоэлектроника
            SelectCategory(label16.Text);
        }

        private void label17_Click(object sender, EventArgs e)
        {
            //Аксессуары
            SelectCategory(label17.Text);
        }
        #endregion
    }
}
