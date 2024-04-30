using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json;
using Client.Models;
using Microsoft.Web.WebView2;

namespace Client.Views
{
    public partial class Profile : Form
    {
        Registration f;
        

        public void formClose( object sender, FormClosedEventArgs e )
        {
            LoadData();
        }


        public void LoadData()
        {
            var config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );

            if(config.AppSettings.Settings["isLogin"].Value == "false")
            {
                notLoginPanel.Location = new Point( 0, 0 );
                notLoginPanel.Visible = true;
                
            }
            if(config.AppSettings.Settings["isLogin"].Value == "true")
            {
                notLoginPanel.Visible= false;
                string command = "LoadUserDataPlease";
                string data = $"{config.AppSettings.Settings["userId"].Value}&";

                SendRequest( command, data );
            }
        }
        public Profile()
        {
            InitializeComponent();
            LoadData();
        }

        private void editBtn_Click( object sender, System.EventArgs e )
        {
            panel1.Visible = true;
            panel1.Location = new Point( 12, 12 );
        }

        private void saveBtn_Click( object sender, System.EventArgs e )
        {
            var config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );

            panel1.Visible = false;
            panel1.Location = new Point( 12, 342 );

            //код для редактирования
            if (string.IsNullOrWhiteSpace( fnameTb.Text ) ||
                string.IsNullOrWhiteSpace( snameTb.Text ) ||
                string.IsNullOrWhiteSpace( logTb.Text ) ||
                string.IsNullOrWhiteSpace( passTb.Text )) 
            {
                MessageBox.Show( "Необходимо заполнить все поля" );
                return;
            }

            string id = config.AppSettings.Settings["userId"].Value;
            string fname = fnameTb.Text;
            string sname = snameTb.Text;
            string log = logTb.Text;
            string pass = passTb.Text;

            string command = "UpdateUserPlease";
            string data = $"{id}&{fname}&{sname}&{log}&{pass}&";
            
            SendRequest(command, data);
        }

        public async void SendRequest(string command, string data)
        {
            try
            {
                string server = "127.0.0.1";
                int port = 4444;

                string sResponse = await SendData( server, port, command, data );

                if(command == "LoadUserDataPlease")
                {
                    if(sResponse == "Error null user")
                    {
                        MessageBox.Show( "При загрузке данных на сервере произошла ошибка(не удалось найти запись пользователя), попробуйте позже", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }
                    else
                    {
                        User user = JsonConvert.DeserializeObject<User>( sResponse );

                        fnameLabel.Text = "Имя: " + user.FName;
                        snameLabel.Text = "Фамилия: " + user.SName;
                        logLabel.Text = "Логин: " + user.Login;
                        passLabel.Text = "Пароль: " + user.Password;
                    }
                }
                else
                {
                    ProcessResponce( sResponse );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show( $"Похоже произошла ошибка\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        public static async Task<string> SendData( string server, int port, string command, string data )
        {
            IPAddress ipAddress = null;
            IPHostEntry ipHostInfo = Dns.GetHostEntry( server );

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
                throw new Exception( "No IPv4 address for server" );

            TcpClient client = new TcpClient();
            await client.ConnectAsync( ipAddress, port );
            NetworkStream networkStream = client.GetStream();

            StreamWriter writer = new StreamWriter( networkStream );
            StreamReader reader = new StreamReader( networkStream );

            writer.AutoFlush = true;
            string requestData = "command=" + command + "&" + data;
            await writer.WriteLineAsync( requestData );
            string response = await reader.ReadLineAsync();
            client.Close();
            return response;
        }

        private void ProcessResponce(string responce)
        {
            if(responce == "Okey")
            {
                MessageBox.Show( "Изменение прошло успешно" );
                this.Controls.Clear();
                this.InitializeComponent();
            }
            else
            {
                MessageBox.Show( "Произошла ошибка" );
            }
        }

        private void button1_Click( object sender, EventArgs e )
        {
            panel1.Visible = false;
            panel1.Location = new Point( 12, 342 );
        }

        private void loginBtn_Click( object sender, EventArgs e )
        {
            f = new Registration( this );
            f.Show();
        }
    }
}
