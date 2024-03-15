using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Client
{
    public partial class Bascket : Form
    {
        int sum = 0;
        int count = 0;
        static List<Product> products;

        public Bascket()
        {
            InitializeComponent();
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
            checkBox.CheckedChanged += async (sender, e) => CheackBoxCheaked(product.Id);
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
            numeric.ValueChanged += async (sender, e) => NumericChanged(product);
            numeric.Minimum = 1;
            numeric.Maximum = product.Count;
            numeric.Parent = card;

            PictureBox remove = new PictureBox();
            remove.Size = new Size(41, 41);
            remove.Location = new Point(881, 119);
            remove.Click += async (sender, e) => RemoveFromBascket(product.Id);
            remove.Image = Properties.Resources.trash;
            remove.SizeMode = PictureBoxSizeMode.StretchImage;
            remove.Parent = card;
            remove.Cursor = Cursors.Hand;

            flowLayoutPanel1.Controls.Add(card);
        }


        private static async void RemoveFromBascket(int id)
        {
            string command = $"RemoveProductPlease&userId={Settings.Default.userId}&productId={id}";

            flowLayoutPanel1.Controls.Clear();

            await LoadProduct(command);

        }


        private static async void NumericChanged(Product product)
        {

        }


        private static async void CheackBoxCheaked(int productId)
        {

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
            string command = $"LoadBasketPlease&id={Settings.Default.userId}";

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
