using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MonopolyWinForms.Login_Signup;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace MonopolyWinForms.GameLogic
{
    public class Chatbox : Panel
    {
        #nullable disable
        private RichTextBox rtbDisplay;
        private RichTextBox richTextBox1;
        private TextBox txtInput;
        private Button btnSend;
        public event Action<string, string> OnSendMessage;
        private Player curPlayer;
        public Chatbox(Player player)
        {
            InitializeComponents();
            curPlayer = player;
        }
        private void InitializeComponents()
        {
            this.Size = new Size(500, 400);
            this.BorderStyle = BorderStyle.FixedSingle;
            rtbDisplay = new RichTextBox
            {
                Location = new Point(10, 10),
                Size = new Size(480, 320),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.White,
                Font = new Font("Arial", 10)
            };
            txtInput = new TextBox
            {
                Location = new Point(10, 360),
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(400, 30),
            };
            btnSend = new Button
            {
                Text = "Gửi",
                Location = new Point(420, 360),
                Size = new Size(70, 30)
            };
            richTextBox1 = new RichTextBox
            {
                Location = new Point(970, 410),
                Size = new Size(480, 110),
                ReadOnly = true,
                Font = new Font("Palatino Linotype", 11.25F, FontStyle.Bold),
                BackColor = Color.FromArgb(245, 235, 221),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = RichTextBoxScrollBars.Vertical
                //richTextBox1.BackColor = Color.FromArgb(245, 235, 221);
                //richTextBox1.BorderStyle = BorderStyle.None;
                //richTextBox1.Font = new Font("Palatino Linotype", 11.25F, FontStyle.Bold);
                //richTextBox1.Location = new Point(970, 410);
                //richTextBox1.Name = "richTextBox1";
                //richTextBox1.ReadOnly = true;
                //richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
                //richTextBox1.Size = new Size(326, 104);
                //richTextBox1.TabIndex = 48;
                //richTextBox1.Text = "";
            };
            this.Controls.Add(richTextBox1);
            // Tăng chiều cao Chatbox panel nếu cần
            this.Size = new Size(500, 500);

            btnSend.Click += BtnSend_Click;
            txtInput.KeyDown += TxtInput_KeyDown;
            this.Controls.Add(rtbDisplay);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnSend);
        }
        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }
        private void SendMessage()
        {
            string message = txtInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                OnSendMessage?.Invoke(Session.UserName, message);
                txtInput.Clear();
            }
        }
        public void AddMessageWithColor(string message, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddMessageWithColor(message, color)));
                return;
            }
            rtbDisplay.SelectionStart = rtbDisplay.TextLength;
            rtbDisplay.SelectionLength = 0;
            rtbDisplay.SelectionColor = color;
            rtbDisplay.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            rtbDisplay.ScrollToCaret();
        }
        //public void AddDiceLog(string msg)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action(() => AddDiceLog(msg)));
        //        return;
        //    }
        //    richTextBox1.SelectionStart = richTextBox1.TextLength;
        //    richTextBox1.SelectionLength = 0;
        //    richTextBox1.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
        //    richTextBox1.ScrollToCaret();
        //}
        public void AddSystemMessage(string message)
        {
            AddMessageWithColor($"[Hệ thống] {message}", Color.Gray);
        }
        public void AddNotificationMessage(string message)
        {
            AddMessageWithColor($"[Thông báo] {message}", Color.DarkGreen);
        }
        public void AddWarningMessage(string message)
        {
            AddMessageWithColor($"[Cảnh báo] {message}", Color.Orange);
        }
        public void AddErrorMessage(string message)
        {
            AddMessageWithColor($"[Lỗi] {message}", Color.Red);
        }
        public void UpdatePlayer(Player player)
        {
            curPlayer = player;
        }
        public void ReceiveMessage(string senderName, string message)
        {
            if (senderName == Session.UserName)
            {
                AddMessageWithColor($"Bạn: {message}", Color.Blue);
            }
            else
            {
                AddMessageWithColor($"{senderName}: {message}", Color.Black);
            }
        }
    }
}
