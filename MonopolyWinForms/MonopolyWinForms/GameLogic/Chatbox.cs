using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class Chatbox : Panel
    {
        #nullable disable
        private RichTextBox rtbDisplay;
        private TextBox txtInput;
        private Button btnSend;
        public event Action<string> OnSendMessage;
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
                rtbDisplay.AppendText($"Player {curPlayer.ID}: {message}{Environment.NewLine}");
                OnSendMessage?.Invoke(message);
                txtInput.Clear();
            }
        }
        public void AddMessage(string message)
        {
            rtbDisplay.AppendText(message + Environment.NewLine);
        }
        public void UpdatePlayer(Player player)
        {
            curPlayer = player;
        }
    }
}
