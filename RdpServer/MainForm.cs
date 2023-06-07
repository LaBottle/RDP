using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

enum State {
    Close = 0,
    Established = 1,
    Listening = 2,
}


namespace RdpServer {
    public partial class MainForm : Form {

        public static ChatServer Chat;

        public MainForm() {
            InitializeComponent();


            Chat = new ChatServer();


            Task.Factory.StartNew(PortDetection, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(MessageReceiver, TaskCreationOptions.LongRunning);

        }
        private void MessageReceiver() {
            Chat.Accept();
            Invoke(new Action(() => {
                ChatLabel.Text = "聊天服务已连接";
                ChatLabel.ForeColor = Color.Blue;
            }));
            while (true) {
                var msg = Chat.Receive();
                Invoke(new Action(() => { ChatRichTextBox.Text += msg; }));
            }
        }

        private void PortDetection() {
            while (true) {
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                var connections = ipProperties.GetActiveTcpConnections();
                var listeners = ipProperties.GetActiveTcpListeners();
                State rdpState = State.Close;
                State ftpState = State.Close;
                State chatState = State.Close;
                string RdpLabelText, FtpLabelText, ChatLabelText;
                Color RdpLabelForeColor, FtpLabelForeColor, ChatLabelForeColor;
                foreach (var endPoint in listeners) {
                    if (endPoint.Port == 3389) {
                        rdpState = State.Listening;
                    }

                    if (endPoint.Port == 21) {
                        ftpState = State.Listening;
                    }

                    if (endPoint.Port == 5555) {
                        chatState = State.Listening;
                    }
                }
                foreach (var tcpConnection in connections) {
                    if (tcpConnection.LocalEndPoint.Port == 3389 &&
                        tcpConnection.State == TcpState.Established) {
                        rdpState = State.Established;
                    }

                    if (tcpConnection.LocalEndPoint.Port == 21 &&
                        tcpConnection.State == TcpState.Established) {
                        ftpState = State.Established;
                    }

                    if (tcpConnection.LocalEndPoint.Port == 5555 &&
                        tcpConnection.State == TcpState.Established) {
                        chatState = State.Established;
                    }
                }

                if (rdpState == State.Established) {
                    RdpLabelText = "远程桌面服务已连接";
                    RdpLabelForeColor = Color.Blue;
                } else if (rdpState == State.Listening) {
                    RdpLabelText = "远程桌面服务已启动";
                    RdpLabelForeColor = Color.Green;
                } else {
                    RdpLabelText = "远程桌面服务已禁用";
                    RdpLabelForeColor = Color.Red;
                }

                if (ftpState == State.Established) {
                    FtpLabelText = "文件服务已连接";
                    FtpLabelForeColor = Color.Blue;
                } else if (ftpState == State.Listening) {
                    FtpLabelText = "文件服务已启动";
                    FtpLabelForeColor = Color.Green;
                } else {
                    FtpLabelText = "文件服务已禁用";
                    FtpLabelForeColor = Color.Red;
                }

                if (chatState == State.Established) {
                    ChatLabelText = "聊天服务已连接";
                    ChatLabelForeColor = Color.Blue;
                } else if (chatState == State.Listening) {
                    ChatLabelText = "聊天服务已启动";
                    ChatLabelForeColor = Color.Green;
                } else {
                    ChatLabelText = "聊天服务已禁用";
                    ChatLabelForeColor = Color.Red;
                }

                Invoke(new Action(() => {
                    RdpLabel.Text = RdpLabelText;
                    RdpLabel.ForeColor = RdpLabelForeColor;
                    FtpLabel.Text = FtpLabelText;
                    FtpLabel.ForeColor = FtpLabelForeColor;
                    ChatLabel.Text = ChatLabelText;
                    ChatLabel.ForeColor = ChatLabelForeColor;
                }));

                Thread.Sleep(1000);
            }
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if (ChatTextBox.Text == string.Empty) {
                MessageBox.Show("请输入聊天内容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var msg = DateTime.Now.ToString(CultureInfo.CurrentCulture) +
                "服务端：\n" + ChatTextBox.Text + "\n";
            try {
                Chat.Send(msg);
                ChatRichTextBox.Text += msg;
            } catch (Exception ex) {
                MessageBox.Show("客户端未连接！\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}