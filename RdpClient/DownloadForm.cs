using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdpClient {
    public partial class DownloadForm : Form {
        public DownloadForm() {
            InitializeComponent();
        }

        private void CommitButton_Click(object sender, EventArgs e) {
            ((MainForm)Owner).Download(PathTextBox.Text);
            Close();
        }
    }
}
