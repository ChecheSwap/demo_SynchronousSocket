using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverSocket
{
    public class Program
    {
        public static string data = default(string);

        private static int requestNumber = 0;
        static void Main(string[] args)
        {
            Console.Title = "My Server";

            startlistening();

            return;
        }

        public static void startlistening()
        {
            byte[] bytes = default(byte[]);

            IPHostEntry iphostinfo = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress ipaddress = iphostinfo.AddressList[0];

            IPEndPoint localendpoint = new IPEndPoint(ipaddress, 11000);

            Socket listener = new Socket(ipaddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localendpoint);

                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("\n@Server: Waiting for a Connection.");

                    Socket handler = listener.Accept();

                    data = string.Empty;

                    while (true)
                    {
                        bytes = new byte[1024];

                        int bytesrec = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesrec);

                        if (data.IndexOf("<!EOF>") > -1)
                        {
                            break;
                        }
                    }

                    data = data.Replace("<!EOF>",String.Empty);
                  

                    Console.WriteLine(Environment.NewLine+"#"+(++requestNumber).ToString()+Environment.NewLine+">>>Text Received: "+data);

                    string resp = $"***Message Received...\n-Message: {data} {Environment.NewLine}";

                    byte[] msg = Encoding.ASCII.GetBytes(resp);

                    handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Both);

                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("\nEnter to continue");
            Console.Read();
        }
    }
}
