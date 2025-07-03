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
            // Chatbox chiếm hết Panel cha (panelChatbox) – bạn đã Dock = Fill từ MainForm
            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.White;

            // ----------- VÙNG HIỂN THỊ TIN NHẮN ------------
            rtbDisplay = new RichTextBox
            {
                Dock = DockStyle.Fill,       // tự co giãn
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Arial", 10),
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            // ----------- VÙNG NHẬP + NÚT GỬI ---------------
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(10, 5, 10, 5)
            };

            txtInput = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 10)
            };

            btnSend = new Button
            {
                Text = "Gửi",
                Dock = DockStyle.Right,
                Width = 70
            };

            // Sự kiện
            btnSend.Click += BtnSend_Click;
            txtInput.KeyDown += TxtInput_KeyDown;

            // Ghép UI
            bottomPanel.Controls.Add(txtInput);
            bottomPanel.Controls.Add(btnSend);
            this.Controls.Add(rtbDisplay);
            this.Controls.Add(bottomPanel);
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
