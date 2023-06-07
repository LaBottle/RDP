using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentFTP;

namespace RdpClient {
    internal static class Program {
        public delegate void ThreadDelegate();
        public static string Ip { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static FtpClient Ftp { get; set; }
        public static ChatClient Chat { get; set; }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
