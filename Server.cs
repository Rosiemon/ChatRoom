using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatRoomServer
{
    class Server
    {
        Dictionary<int, string> availableClients = new Dictionary<int, string>();
        TcpListener server;
        Byte[] bytes = new Byte[256];
        String data = null;
        Int32 port = 8800;

        public Server()
        {
            TcpListener server = new TcpListener(port);
        }


        public static void Connect()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 8800;
                IPAddress localAddress = IPAddress.Parse("192.168.0.109");
                server = new TcpListener(localAddress, port);
                server.Start();
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data); 

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read()
        }
    }
}