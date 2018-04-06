using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clientSocketWF
{
    public class socketClientEvent
    {                

        private IPAddress ipaddress = default(IPAddress);

        private int port = 0;

        private IPEndPoint remoteEP = default(IPEndPoint);

        private byte[] bytes = default(byte[]);

        private Socket sender = default(Socket);

        private string message = default(string);
        public socketClientEvent(IPAddress ip, int port)           
        {
            this.bytes = default(byte []);
            this.ipaddress = ip;
            this.port = port;
            this.remoteEP = new IPEndPoint(this.ipaddress, this.port);
        }

        public bool connect()
        {
            bool flag = false;

            try
            {
                this.sender = new Socket(ipaddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.sender.Connect(this.remoteEP);
                flag = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return flag;
        }

        public string  receive()
        {
            this.message = default(string);

            try
            {
                this.bytes = new byte[1024];

                int bytesrec = sender.Receive(bytes);

                this.message = Encoding.ASCII.GetString(this.bytes, 0, bytesrec);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return this.message;
        }
        public void send(string xmsg)
        {
            try
            {                     
                byte [] msg = Encoding.ASCII.GetBytes(xmsg+"<!EOF>");

                int bytessent = sender.Send(msg);          
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.ToString());
            }
        }

        public void close()
        {
            try
            {
                this.sender.Shutdown(SocketShutdown.Both);

                this.sender.Close();
            }
            catch(SocketException se)
            {
                MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
    }
}

