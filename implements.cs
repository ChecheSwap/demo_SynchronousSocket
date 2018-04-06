using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clientSocketWF
{
    public partial class Form1 : Form
    {

        private socketClientEvent sce = default(socketClientEvent);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());

            this.sce = new socketClientEvent(entry.AddressList[0], 11000);
        }

        private void routine()
        {
            if (this.sce.connect())
            {
                this.sce.send(this.rtbsend.Text.Trim());
                this.rtbrespuesta.Text += Environment.NewLine +">>"+ this.sce.receive();
                this.sce.close();
            }
            else
            {
                MessageBox.Show("Problema de coneccion");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.routine();
        }
    }
}
