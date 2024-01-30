
namespace TaifexEmulator
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.StartButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipTxt = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.sendTxBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.msgLabel = new System.Windows.Forms.RichTextBox();
            this.sndTelBtn = new System.Windows.Forms.Button();
            this.telType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(36, 33);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(109, 31);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start Listen";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.Start_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ipLabel);
            this.flowLayoutPanel1.Controls.Add(this.ipTxt);
            this.flowLayoutPanel1.Controls.Add(this.portLabel);
            this.flowLayoutPanel1.Controls.Add(this.portTxt);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(175, 30);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(345, 36);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ipLabel.Location = new System.Drawing.Point(3, 10);
            this.ipLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(24, 15);
            this.ipLabel.TabIndex = 0;
            this.ipLabel.Text = "IP:";
            // 
            // ipTxt
            // 
            this.ipTxt.Location = new System.Drawing.Point(30, 5);
            this.ipTxt.Margin = new System.Windows.Forms.Padding(0);
            this.ipTxt.Name = "ipTxt";
            this.ipTxt.Size = new System.Drawing.Size(155, 25);
            this.ipTxt.TabIndex = 5;
            this.ipTxt.Text = "0.0.0.0";
            this.ipTxt.TextChanged += new System.EventHandler(this.ipTxt_TextChanged);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(195, 10);
            this.portLabel.Margin = new System.Windows.Forms.Padding(10, 5, 3, 0);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(35, 15);
            this.portLabel.TabIndex = 6;
            this.portLabel.Text = "Port:";
            // 
            // portTxt
            // 
            this.portTxt.Location = new System.Drawing.Point(233, 5);
            this.portTxt.Margin = new System.Windows.Forms.Padding(0);
            this.portTxt.Name = "portTxt";
            this.portTxt.Size = new System.Drawing.Size(92, 25);
            this.portTxt.TabIndex = 7;
            this.portTxt.Text = "30004";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(36, 90);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(109, 31);
            this.sendButton.TabIndex = 5;
            this.sendButton.Text = "Send Message";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // sendTxBox
            // 
            this.sendTxBox.Location = new System.Drawing.Point(175, 93);
            this.sendTxBox.Multiline = true;
            this.sendTxBox.Name = "sendTxBox";
            this.sendTxBox.Size = new System.Drawing.Size(727, 25);
            this.sendTxBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Receive:";
            // 
            // msgLabel
            // 
            this.msgLabel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.msgLabel.Font = new System.Drawing.Font("微軟正黑體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.msgLabel.Location = new System.Drawing.Point(36, 240);
            this.msgLabel.Name = "msgLabel";
            this.msgLabel.Size = new System.Drawing.Size(866, 229);
            this.msgLabel.TabIndex = 1;
            this.msgLabel.Text = "";
            this.msgLabel.WordWrap = false;
            this.msgLabel.TextChanged += new System.EventHandler(this.msgLabel_TextChanged);
            // 
            // sndTelBtn
            // 
            this.sndTelBtn.Location = new System.Drawing.Point(36, 143);
            this.sndTelBtn.Name = "sndTelBtn";
            this.sndTelBtn.Size = new System.Drawing.Size(109, 30);
            this.sndTelBtn.TabIndex = 8;
            this.sndTelBtn.Text = "送出電文";
            this.sndTelBtn.UseVisualStyleBackColor = true;
            this.sndTelBtn.Click += new System.EventHandler(this.sndTelBtn_Click);
            // 
            // telType
            // 
            this.telType.FormattingEnabled = true;
            this.telType.Items.AddRange(new object[] {
            "RX08",
            "R14",
            "RX14",
            "RX20"});
            this.telType.Location = new System.Drawing.Point(175, 143);
            this.telType.Name = "telType";
            this.telType.Size = new System.Drawing.Size(134, 23);
            this.telType.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 491);
            this.Controls.Add(this.telType);
            this.Controls.Add(this.sndTelBtn);
            this.Controls.Add(this.msgLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sendTxBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.StartButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "TaifexEmulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			System.Environment.Exit(0);
		}

		#endregion
		private System.Windows.Forms.BindingSource bindingSource1;
		public System.Windows.Forms.Button StartButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label ipLabel;
		private System.Windows.Forms.TextBox ipTxt;
		private System.Windows.Forms.Label portLabel;
		private System.Windows.Forms.TextBox portTxt;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.RichTextBox msgLabel;
		public System.Windows.Forms.TextBox sendTxBox;
        public System.Windows.Forms.Button sndTelBtn;
        public System.Windows.Forms.ComboBox telType;
    }
}

