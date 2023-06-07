using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdpClient {
    public partial class ChatForm : Form {
        public ChatForm() {
            InitializeComponent();
            Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
        }


        private void Listen() {
            while (true) {
                var msg = Program.Chat.Receive();
                if (!string.IsNullOrWhiteSpace(msg)) {
                    Invoke(new Action(() => { ChatRichTextBox.Text += msg; }));
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if (ChatTextBox.Text == string.Empty) {
                MessageBox.Show("请输入聊天内容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var msg = DateTime.Now.ToString(CultureInfo.CurrentCulture) +
                "客户端：\n" + ChatTextBox.Text + "\n";
            Program.Chat.Send(msg);

            ChatRichTextBox.Text += msg;
        }

        private void ChatForm_Load(object sender, EventArgs e) {
            var msg = DateTime.Now.ToString(CultureInfo.CurrentCulture) + "客户端已连接\n";
            Program.Chat.Send(msg);

            ChatRichTextBox.Text += msg;
        }
    }
}