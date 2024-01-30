using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaifexEmulator
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
		private float x;
		private float y;

		public Form1()
        {
            InitializeComponent();
            form1 = this;
            x = this.Width;
            y = this.Height;
            setTag(this);
        }

		private void setTag(Control cons)
		{
			foreach (Control con in cons.Controls)
			{
				con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
				if (con.Controls.Count > 0)
				{
					setTag(con);
				}
			}
		}

		private void setControls(float newX, float newY, Control cons)
		{
			foreach (Control con in cons.Controls)
			{
				if (con.Tag != null)
				{
					string[] mytag = con.Tag.ToString().Split(new char[] { ';' });

					con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newX);
					con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newY);
					con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newX);
					con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newY);
					Single currentSize = System.Convert.ToSingle(mytag[4]) * newY;
					//con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
					if (con.Controls.Count > 0)
					{
						setControls(newX, newY, con);
					}
				}
			}
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			float newX = (this.Width) / x;
			float newY = (this.Height) / y;
			setControls(newX, newY, this);
		}

		private void Form1_Load(object sender, EventArgs e)
        {

        }

		private void Start_Click(object sender, EventArgs e)
		{
            Receive.SocketInit(ipTxt.Text, int.Parse(portTxt.Text));
		}

		private void msgLabel_TextChanged(object sender, EventArgs e)
		{
            form1.msgLabel.SelectionStart = Form1.form1.msgLabel.Text.Length;
            form1.msgLabel.ScrollToCaret();
        }

		private void sendButton_Click(object sender, EventArgs e)
		{
            Receive.SendMessage(Encoding.ASCII.GetBytes(form1.sendTxBox.Text), form1.sendTxBox.TextLength);
		}

        private void ipTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void sndTelBtn_Click(object sender, EventArgs e)
        {
			byte msgType = 0;
			switch (telType.Text)
            {
				case "RX02": msgType = 202; break;
				case "RX03": msgType = 203; break;
				case "RX08": msgType = 208; break;
				case "R14" : msgType = 114; break;
				case "RX14": msgType = 214; break;
				case "RX41": msgType = 241; break;
				case "RX20": msgType = 220; break;
				default: msgType = 0; break;
			}
			if (msgType != 0)
				Receive.BuildSendBuff(msgType, null);
        }
    }
}
