using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace Client
{
    public partial class Bascket : Form
    {
        static List<Product> products;
        static Dictionary<Product, int> priceMap = new Dictionary<Product, int>();
        static decimal numValue = 1;
        public Bascket()
        {
            InitializeComponent();
        }


        public static void LoadPrice()
        {
            int totalPrice = 0;
            int totalTovars = 0;
            foreach(var value in priceMap)
            {
                totalTovars += value.Value;
                int price = int.Parse(value.Key.Price.Replace( " ", "" ));
                totalPrice += price * value.Value;
            }
            label2.Text = $"Всего товаров: {totalTovars}\r\n\r\nИтоговая цена: {totalPrice} ₽";
        }


        public static async Task LoadProduct(string command)
        {
            try
            {
                string server = "127.0.0.1";
                int port = 4444;
                
                string sResponse = await SendRequest(server, port, command);
               
                if(sResponse == "Empty")
                {
                    Label label = new Label();
                    label.Text = "Ваша корзина пуста";
                    label.AutoSize = true;
                    label.Font = new Font("Arial", 20);
                    label.Parent = flowLayoutPanel1;
                    return;
                }

                products = JsonConvert.DeserializeObject<List<Product>>(sResponse);

                foreach(var product in products)
                {
                    await GenerateCards(product);
                    var price = product.Price.Replace( " ", "" );
                    priceMap.Add( product, 1 );
                    LoadPrice();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private static Image DecodeBase64Image(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }
        

        static async Task GenerateCards(Product product)
        {
            Panel card = new Panel();
            card.Location = new Point(20, 20);
            card.Size = new Size(962, 200);

            CheckBox checkBox = new CheckBox();
            checkBox.Location = new Point(10, 85);
            checkBox.Size = new Size(30, 30);
            checkBox.Checked = true;
            checkBox.CheckedChanged += async (sender, e) => await CheackBoxCheaked(product, checkBox);
            checkBox.Parent = card;

            PictureBox picture = new PictureBox();
            picture.Size = new Size(200, 200);
            picture.Location = new Point(60, 0);
            picture.Parent = card;
            picture.Image = DecodeBase64Image(product.ImgBase64);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            Label nameLabel = new Label();
            nameLabel.Text = product.Name;
            nameLabel.Font = new Font("Arial", 12);
            nameLabel.Location = new Point(276, 1);
            nameLabel.Size = new Size(545, 100);
            nameLabel.Padding = new Padding(10, 20, 0, 0);
            nameLabel.Parent = card;

            Label priceLabel = new Label();
            priceLabel.Text = product.Price + " ₽";
            priceLabel.Font = new Font("Arial", 12);
            priceLabel.Location = new Point(276, 99);
            priceLabel.Size = new Size(545, 100);
            priceLabel.Padding = new Padding(10, 20, 0, 0);
            priceLabel.Parent = card;

            NumericUpDown numeric = new NumericUpDown();
            numeric.Location = new Point(871, 40);
            numeric.Size = new Size(61, 30);
            numeric.Font = new Font("Arial", 12);
            numeric.ValueChanged += async (sender, e) => await NumericChanged(product, numeric);
            numeric.Minimum = numValue;
            numeric.Maximum = product.Count;
            numeric.Parent = card;

            PictureBox remove = new PictureBox();
            remove.Size = new Size(41, 41);
            remove.Location = new Point(881, 119);
            remove.Click += async (sender, e) => await RemoveFromBascket(product);
            remove.Image = Properties.Resources.trash;
            remove.SizeMode = PictureBoxSizeMode.StretchImage;
            remove.Parent = card;
            remove.Cursor = Cursors.Hand;

            flowLayoutPanel1.Controls.Add(card);
        }


        private static async Task RemoveFromBascket(Product product)
        {
            var config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );

            string command = $"RemoveProductPlease&userId={config.AppSettings.Settings["userId"].Value}&productId={product.Id}";

            flowLayoutPanel1.Controls.Clear();

            priceMap.Clear();
            await LoadProduct( command );
            LoadPrice();
        }


        private static async Task NumericChanged(Product product, NumericUpDown numeric )
        {
            if(numeric.Value > numValue)
            {
                numValue = numeric.Value;
                priceMap[product]++;
                LoadPrice();
            }
            if(numeric.Value < numValue)
            {
                numValue = numeric.Value;
                priceMap[product]--;
                LoadPrice();
            }
        }


        private static async Task CheackBoxCheaked(Product product, CheckBox checkBox)
        {
            if( checkBox.Checked == true)
            {
                priceMap.Add( product, 1 );
                LoadPrice();
            }
            if(checkBox.Checked == false)
            {
                priceMap.Remove(product);
                LoadPrice();
            }
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

        private async void Bascket_Load(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );

            string command = $"LoadBasketPlease&id={config.AppSettings.Settings["userId"].Value}";

            await LoadProduct(command);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ваш заказ принят в обработку.\nОжидайте сообщения о готовности на ваш номер телефона");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ваш заказ принят в обработку.\nОжидайте сообщения о готовности на ваш номер телефона");
        }
    }
}
