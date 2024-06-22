using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using Server.Models;
using Server.Controllers;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class AsyncService
    {
        private IPAddress ipAddress;
        private int port;

        public AsyncService(int port)
        {
            this.port = port;
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
            this.ipAddress = null;

            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {

                if (ipHostInfo.AddressList[i].AddressFamily ==
                  AddressFamily.InterNetwork)
                {
                    this.ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }

            if (this.ipAddress == null)
                throw new Exception("No IPv4 address for server");
        }

        public async Task Run()
        {
            TcpListener listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();

            Console.Write("Server is running");
            Console.WriteLine(" on port " + this.port);
            Console.WriteLine("Hit <enter> to stop service");

            while (true)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Task t = Process(tcpClient);
                    await t;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private async Task Process(TcpClient tcpClient)
        {
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();

            Console.WriteLine("Received connection request from " + clientEndPoint);

            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(networkStream);
                StreamWriter writer = new StreamWriter(networkStream);
                writer.AutoFlush = true;

                while (true)
                {
                    string request = await reader.ReadLineAsync();

                    if (request != null)
                    {
                        Console.WriteLine("Received service request: " + request);
                        string response = Response(request);

                        //Console.WriteLine("Computed response is: " + response + "\n");
                        await writer.WriteLineAsync(response);
                    }

                    else
                        break;
                }
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        private static string Response(string request)
        {
            string[] data = request.Split("&");

            string command = data[0].Substring(8);

            Console.WriteLine(command);

            if (command == "LoadProductsPlease")
            {
                List<Product> products = new DataBase().Product.ToList();
                string serializedProducts = JsonConvert.SerializeObject(products);
                Console.WriteLine("loadproductplease complete");
                return serializedProducts;
            }


            if (command == "RegistrationPlease")
            {
                string fname = data[1].Substring(6);
                string sname = data[2].Substring(6);
                string login = data[3].Substring(6);
                string pass = data[4].Substring(9);

                int id = DbController.Registration(fname, sname, login, pass);
                Console.WriteLine("registrationplease complete");
                return id.ToString();
            }


            if (command == "LoginPlease")
            {
                string login = data[1].Substring(6);
                string pass = data[2].Substring(5);

                User user = DbController.Login(login, pass);
                Console.WriteLine("loginplease complete");
                if (user != null)
                {
                    return user.Id.ToString();
                }
                else
                {
                    return "Login error";
                }
            }


            if (command == "LoadBasketPlease")
            {
                string id = data[1].Substring(3);

                List<Product> products = DbController.Basket(int.Parse(id));
                Console.WriteLine("loadbasketplease complete");
                if (products != null)
                {
                    string serializedProducts = JsonConvert.SerializeObject(products);

                    return serializedProducts;
                }
                else
                {
                    return "Empty";
                }
            }


            if (command == "AddToBasketPlease")
            {
                try
                {
                    string userId = data[1].Substring(7);
                    string pruductId = data[2].Substring(10);
                    Console.WriteLine(userId);
                    Console.WriteLine(pruductId);

                    DbController.AddToBasket(userId, pruductId);
                    Console.WriteLine("addtobasketplease complete");
                    return "Okey";
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return ex.Message;
                }
            }


            if (command == "RemoveProductPlease")
            {
                try
                {
                    int.TryParse( data[1].Substring( 7 ), out int userId );
                    string test = data[2].Substring( 10 );
                    int.TryParse( test, out int productId );
                    DbController.RemoveProduct(userId, productId);

                    List<Product> products = DbController.Basket(userId);

                    if (products != null)
                    {
                        string serializedProducts = JsonConvert.SerializeObject(products);
                        Console.WriteLine("removeproductplease complete");
                        return serializedProducts;
                    }
                    else
                    {
                        Console.WriteLine("removeproductplease error");
                        return "Empty";
                    }
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }


            if (command == "UpdateUserPlease")
            {
                string id = data[1];
                string fname = data[2];
                string sname = data[3];
                string log = data[4];
                string pass = data[5];

                User user = new()
                {
                    FName = fname,
                    SName = sname,
                    Login = log,
                    Password = pass
                };

                DbController.EditUser( id, user );
                Console.WriteLine("updateuserplease complete");
                return "Okey";
            }


            if (command == "LoadUserDataPlease")
            {
                User user = DbController.GetUser( data[1] );
                if(user != null)
                {
                    string serializedUser= JsonConvert.SerializeObject( user );
                    Console.WriteLine("loaduserdataplease complete");
                    return serializedUser;
                }
                else
                {
                    return "Error null user";
                }
            }

            else
            {
                return "ERROR, UNKNOWN COMMAND";
            }
        }
    }
}
