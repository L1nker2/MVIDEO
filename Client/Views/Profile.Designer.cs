﻿namespace Client.Views
{
    partial class Profile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.fnameLabel = new System.Windows.Forms.Label();
            this.snameLabel = new System.Windows.Forms.Label();
            this.logLabel = new System.Windows.Forms.Label();
            this.passLabel = new System.Windows.Forms.Label();
            this.editBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.passTb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.logTb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.snameTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fnameTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.notLoginPanel = new System.Windows.Forms.Panel();
            this.loginBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.notLoginPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(692, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 60);
            this.label1.TabIndex = 1;
            this.label1.Text = "Наши магазины:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fnameLabel
            // 
            this.fnameLabel.Location = new System.Drawing.Point(69, 20);
            this.fnameLabel.Name = "fnameLabel";
            this.fnameLabel.Size = new System.Drawing.Size(208, 28);
            this.fnameLabel.TabIndex = 2;
            this.fnameLabel.Text = "Имя";
            this.fnameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // snameLabel
            // 
            this.snameLabel.Location = new System.Drawing.Point(69, 88);
            this.snameLabel.Name = "snameLabel";
            this.snameLabel.Size = new System.Drawing.Size(208, 28);
            this.snameLabel.TabIndex = 3;
            this.snameLabel.Text = "Фамилия";
            this.snameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logLabel
            // 
            this.logLabel.Location = new System.Drawing.Point(69, 156);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(208, 28);
            this.logLabel.TabIndex = 4;
            this.logLabel.Text = "Логин";
            this.logLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // passLabel
            // 
            this.passLabel.Location = new System.Drawing.Point(69, 224);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(208, 28);
            this.passLabel.TabIndex = 5;
            this.passLabel.Text = "Пароль";
            this.passLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // editBtn
            // 
            this.editBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(18)))), ((int)(((byte)(53)))));
            this.editBtn.ForeColor = System.Drawing.Color.White;
            this.editBtn.Location = new System.Drawing.Point(69, 286);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(200, 50);
            this.editBtn.TabIndex = 6;
            this.editBtn.Text = "Изменить";
            this.editBtn.UseVisualStyleBackColor = false;
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.saveBtn);
            this.panel1.Controls.Add(this.passTb);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.logTb);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.snameTb);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.fnameTb);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 342);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 389);
            this.panel1.TabIndex = 7;
            this.panel1.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(18)))), ((int)(((byte)(53)))));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(33, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 50);
            this.button1.TabIndex = 9;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(18)))), ((int)(((byte)(53)))));
            this.saveBtn.ForeColor = System.Drawing.Color.White;
            this.saveBtn.Location = new System.Drawing.Point(33, 275);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(200, 50);
            this.saveBtn.TabIndex = 8;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.UseVisualStyleBackColor = false;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // passTb
            // 
            this.passTb.Location = new System.Drawing.Point(7, 239);
            this.passTb.Name = "passTb";
            this.passTb.Size = new System.Drawing.Size(250, 26);
            this.passTb.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "Пароль";
            // 
            // logTb
            // 
            this.logTb.Location = new System.Drawing.Point(7, 171);
            this.logTb.Name = "logTb";
            this.logTb.Size = new System.Drawing.Size(250, 26);
            this.logTb.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Логин";
            // 
            // snameTb
            // 
            this.snameTb.Location = new System.Drawing.Point(7, 103);
            this.snameTb.Name = "snameTb";
            this.snameTb.Size = new System.Drawing.Size(250, 26);
            this.snameTb.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Фамилия";
            // 
            // fnameTb
            // 
            this.fnameTb.Location = new System.Drawing.Point(7, 35);
            this.fnameTb.Name = "fnameTb";
            this.fnameTb.Size = new System.Drawing.Size(250, 26);
            this.fnameTb.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Имя";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.webView21);
            this.panel2.Location = new System.Drawing.Point(337, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1010, 521);
            this.panel2.TabIndex = 8;
            // 
            // notLoginPanel
            // 
            this.notLoginPanel.Controls.Add(this.loginBtn);
            this.notLoginPanel.Controls.Add(this.label6);
            this.notLoginPanel.Location = new System.Drawing.Point(0, 0);
            this.notLoginPanel.Name = "notLoginPanel";
            this.notLoginPanel.Size = new System.Drawing.Size(337, 621);
            this.notLoginPanel.TabIndex = 9;
            this.notLoginPanel.Visible = false;
            // 
            // loginBtn
            // 
            this.loginBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(18)))), ((int)(((byte)(53)))));
            this.loginBtn.ForeColor = System.Drawing.Color.White;
            this.loginBtn.Location = new System.Drawing.Point(69, 255);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(200, 65);
            this.loginBtn.TabIndex = 1;
            this.loginBtn.Text = "Войти\r\nЗарегистрироваться";
            this.loginBtn.UseVisualStyleBackColor = false;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Location = new System.Drawing.Point(3, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(323, 76);
            this.label6.TabIndex = 0;
            this.label6.Text = "Похоже вы не выполнили вход для просмотра своего профиля =(";
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView21.Location = new System.Drawing.Point(0, 0);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(1010, 521);
            this.webView21.Source = new System.Uri("https://www.mvideo.ru/shops/store-list?from=header", System.UriKind.Absolute);
            this.webView21.TabIndex = 0;
            this.webView21.ZoomFactor = 1D;
            // 
            // Profile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1348, 621);
            this.Controls.Add(this.notLoginPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.logLabel);
            this.Controls.Add(this.snameLabel);
            this.Controls.Add(this.fnameLabel);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Profile";
            this.Text = "Profile";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.notLoginPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label fnameLabel;
        private System.Windows.Forms.Label snameLabel;
        private System.Windows.Forms.Label logLabel;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.TextBox passTb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox logTb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox snameTb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fnameTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button loginBtn;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Panel notLoginPanel;
    }
}