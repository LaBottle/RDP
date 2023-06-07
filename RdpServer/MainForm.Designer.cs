namespace RdpServer {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.RdpLabel = new System.Windows.Forms.Label();
            this.ChatRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ChatTextBox = new System.Windows.Forms.TextBox();
            this.FtpLabel = new System.Windows.Forms.Label();
            this.ChatLabel = new System.Windows.Forms.Label();
            this.SendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RdpLabel
            // 
            this.RdpLabel.AutoSize = true;
            this.RdpLabel.Location = new System.Drawing.Point(22, 36);
            this.RdpLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.RdpLabel.Name = "RdpLabel";
            this.RdpLabel.Size = new System.Drawing.Size(142, 24);
            this.RdpLabel.TabIndex = 1;
            this.RdpLabel.Text = "正在加载...";
            // 
            // ChatRichTextBox
            // 
            this.ChatRichTextBox.Location = new System.Drawing.Point(28, 236);
            this.ChatRichTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.ChatRichTextBox.Name = "ChatRichTextBox";
            this.ChatRichTextBox.ReadOnly = true;
            this.ChatRichTextBox.Size = new System.Drawing.Size(968, 324);
            this.ChatRichTextBox.TabIndex = 2;
            this.ChatRichTextBox.Text = "";
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Location = new System.Drawing.Point(24, 598);
            this.ChatTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.Size = new System.Drawing.Size(816, 35);
            this.ChatTextBox.TabIndex = 3;
            // 
            // FtpLabel
            // 
            this.FtpLabel.AutoSize = true;
            this.FtpLabel.Location = new System.Drawing.Point(24, 102);
            this.FtpLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.FtpLabel.Name = "FtpLabel";
            this.FtpLabel.Size = new System.Drawing.Size(0, 24);
            this.FtpLabel.TabIndex = 4;
            this.FtpLabel.Text = "正在加载...";
            // 
            // ChatLabel
            // 
            this.ChatLabel.AutoSize = true;
            this.ChatLabel.Location = new System.Drawing.Point(22, 170);
            this.ChatLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ChatLabel.Name = "ChatLabel";
            this.ChatLabel.Size = new System.Drawing.Size(0, 24);
            this.ChatLabel.TabIndex = 5;
            this.ChatLabel.Text = "正在加载...";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(850, 598);
            this.SendButton.Margin = new System.Windows.Forms.Padding(4);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(150, 42);
            this.SendButton.TabIndex = 6;
            this.SendButton.Text = "发送";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.SendButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 654);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ChatLabel);
            this.Controls.Add(this.FtpLabel);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.ChatRichTextBox);
            this.Controls.Add(this.RdpLabel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Text = "RDP服务端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RdpLabel;
        private System.Windows.Forms.RichTextBox ChatRichTextBox;
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.Label FtpLabel;
        private System.Windows.Forms.Label ChatLabel;
        private System.Windows.Forms.Button SendButton;
    }
}

