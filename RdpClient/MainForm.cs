using AxMSTSCLib;
using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentFTP;
using System.Threading;

namespace RdpClient {
    public partial class MainForm : Form {
        private AxMsRdpClient11NotSafeForScripting Rdp { get; set; }
        private ChatForm ChatForm { get; set; }
        private DownloadForm DownloadForm { get; set; }

        public MainForm() {
            InitializeComponent();
        }

        ~MainForm() {
            if (Program.Ftp != null && !Program.Ftp.IsDisposed) {
                Program.Ftp.Dispose();
            }

            if (Program.Chat != null && !Program.Chat.IsDisposed) {
                Program.Chat.Dispose();
            }

            if (Rdp != null && !Rdp.IsDisposed) {
                Rdp.Dispose();
                Controls.Remove(Rdp);
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e) {
            IpTextBox.Enabled = true;
            UserNameTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
            ConnectButton.Enabled = true;
            ConnectButton.Text = "连接";
            DisconnectButton.Enabled = false;
            UploadButton.Enabled = false;
            UploadDirButton.Enabled = false;
            DownloadButton.Enabled = false;
            ChatButton.Enabled = false;

            if (!IPAddress.TryParse(IpTextBox.Text, out IPAddress ip)) {
                MessageBox.Show("请输入正常的IP！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (UserNameTextBox.Text == string.Empty) {
                MessageBox.Show("请输入用户名！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (PasswordTextBox.Text == string.Empty) {
                MessageBox.Show("请输入密码！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.Ip = IpTextBox.Text;
            Program.UserName = UserNameTextBox.Text;
            Program.Password = PasswordTextBox.Text;


            if (Rdp != null && !Rdp.IsDisposed) {
                Rdp.Dispose();
                Controls.Remove(Rdp);
            }

            Task.Run(() => {
                if (Program.Ftp != null && !Program.Ftp.IsDisposed) {
                    Program.Ftp.Dispose();
                }

                Program.Ftp = new FtpClient(Program.Ip, Program.UserName, Program.Password);
                Program.Ftp.AutoConnect();
                if (Program.Ftp.IsConnected) {
                    Invoke(new Action(() => {
                        UploadButton.Enabled = true;
                        UploadDirButton.Enabled = true;
                        DownloadButton.Enabled = true;
                    }));
                } else {
                    MessageBox.Show("无法连接到FTP服务器！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            });

            Task.Run(() => {
                if (Program.Chat != null && !Program.Chat.IsDisposed) {
                    Program.Chat.Dispose();
                }

                try {
                    Program.Chat = new ChatClient();
                    Invoke(new Action(() => { ChatButton.Enabled = true; }));
                } catch (Exception ex) {
                    MessageBox.Show("无法连接到聊天服务！原因是：\n" + ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            });

            var rdp = new AxMsRdpClient11NotSafeForScripting {
                Size = new Size(1280, 720),
                Location = new Point(22, 88),
                Margin = new Padding(2, 2, 2, 2)
            };
            Controls.Add(rdp);
            Rdp = rdp;
            rdp.Server = Program.Ip;
            rdp.UserName = Program.UserName;
            rdp.AdvancedSettings2.ClearTextPassword = Program.Password;

            var clientNonScriptable = (IMsRdpClientNonScriptable5)rdp.GetOcx();
            rdp.AdvancedSettings9.EnableCredSspSupport = true;
            rdp.ConnectingText = $"正在连接[{Program.Ip}]，请稍等... ";
            rdp.Connect();
            if (rdp.Connected != 2) {
                MessageBox.Show("连接失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            IpTextBox.Enabled = false;
            UserNameTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            ConnectButton.Enabled = true;
            ConnectButton.Text = "重新连接";
            DisconnectButton.Enabled = true;
        }

        private void UploadButton_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Title = "请选择文件";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string file = dialog.FileName;
            var ftpState = Program.Ftp.UploadFile(file, Path.GetFileName(file));
            if (ftpState == FtpStatus.Failed) {
                MessageBox.Show("传输过程中发送错误或上传文件不存在，请检查您的网络情况以及文件权限，必要时请重启连接。",
                    "上传文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (ftpState == FtpStatus.Skipped) {
                MessageBox.Show("文件已存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else if (ftpState == FtpStatus.Success) {
                MessageBox.Show("文件上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UploadDirButton_Click(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            dialog.SelectedPath = @"C:\";
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string dir = dialog.SelectedPath;
            var ftpStates = Program.Ftp.UploadDirectory(dir, Path.GetFileName(dir));
            FtpStatus ftpState = FtpStatus.Success;
            foreach (var s in ftpStates) {
                if (s.IsFailed) {
                    ftpState = FtpStatus.Failed; break;
                } else if (s.IsSkipped) {
                    ftpState = FtpStatus.Skipped;
                }
            }
            if (ftpState == FtpStatus.Failed) {
                MessageBox.Show("传输过程中发送错误或上传文件不存在，请检查您的网络情况以及文件权限，必要时请重启连接。",
                    "上传文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (ftpState == FtpStatus.Skipped) {
                MessageBox.Show("文件已存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else if (ftpState == FtpStatus.Success) {
                MessageBox.Show("文件上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DownloadButton_Click(object sender, EventArgs e) {
            if (DownloadForm == null || DownloadForm.IsDisposed) {
                DownloadForm = new DownloadForm();
                DownloadForm.Show(this);
            } else {
                DownloadForm.Focus();
            }
        }

        public void Download(string path) {
            try {
                var ftpState = Program.Ftp.DownloadFile(@"C:/abc/" + path, path);
                if (ftpState == FtpStatus.Success) {
                    MessageBox.Show("文件下载成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {
                    MessageBox.Show("传输过程中发送错误，请检查您的网络情况以及文件权限，必要时请重启连接。",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MessageBox.Show("该文件不存在！",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChatButton_Click(object sender, EventArgs e) {
            if (ChatForm == null || ChatForm.IsDisposed) {
                ChatForm = new ChatForm();
                ChatForm.Show(this);
            } else {
                ChatForm.Focus();
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e) {
            IpTextBox.Enabled = true;
            UserNameTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
            ConnectButton.Enabled = true;
            ConnectButton.Text = "连接";
            DisconnectButton.Enabled = false;
            UploadButton.Enabled = false;
            UploadDirButton.Enabled = false;
            DownloadButton.Enabled = false;
            ChatButton.Enabled = false;

            if (DownloadForm != null && !DownloadForm.IsDisposed) {
                DownloadForm.Close();
                DownloadForm = null;
            }

            if (ChatForm != null && !ChatForm.IsDisposed) {
                ChatForm.Close();
                ChatForm = null;
            }

            if (Program.Ftp != null && !Program.Ftp.IsDisposed) {
                try { Program.Ftp.Disconnect(); } finally {
                    Program.Ftp.Dispose();
                }
                Program.Ftp = null;
            }

            if (Program.Chat != null && !Program.Chat.IsDisposed) {
                try { Program.Chat.Disconnect(); } finally {
                    Program.Chat.Dispose();
                }
                Program.Chat = null;
            }

            if (Rdp != null && !Rdp.IsDisposed) {

                try { Rdp.Disconnect(); } finally {
                    Rdp.Dispose();
                    Rdp = null;
                    Controls.Remove(Rdp);
                }
            }
        }


    }
}