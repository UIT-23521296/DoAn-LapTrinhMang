using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.GameLogic
{
    public class Chatbox : Panel
    {
        #nullable disable
        private RichTextBox rtbDisplay;
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
