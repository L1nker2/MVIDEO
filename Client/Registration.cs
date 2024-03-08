﻿using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string fname = textBox1.Text;
            string sname = textBox2.Text;
            string login = textBox4.Text;
            string password = textBox3.Text;
            string passTwo = textBox5.Text;

            if(button1.Text != "Войти")
            {
                if(fname == "" && sname == "" && login == "" && passTwo == "")
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                if (password != passTwo)
                {
                    MessageBox.Show("Пароли не совпадают");
                    return;
                }
            }

            if(button1.Text != "Войти")
            {
                try
                {
                    //registration
                    string server = "127.0.0.1";
                    int port = 4444;
                    string command = $"RegistrationPlease&fname={fname}&sname={sname}&login={login}&password={password}";

                    string sResponse = await Katalog.SendRequest(server, port, command);

                    Settings.Default.isLogin = true;
                    Settings.Default.userId = int.Parse(sResponse);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                try
                {
                    //login
                    string server = "127.0.0.1";
                    int port = 4444;
                    string command = $"LoginPlease&login={login}&pass={password}";

                    string sResponse = await Katalog.SendRequest(server, port, command);

                    if(sResponse != "Login error")
                    {
                        Settings.Default.isLogin = true;
                        Settings.Default.userId = int.Parse(sResponse);
                        this.Close();
                    }

                    else
                    {
                        MessageBox.Show("Не удалось войти");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox5.Enabled = false;
            button1.Text = "Войти";
            button2.Visible = false;
        }
    }
}