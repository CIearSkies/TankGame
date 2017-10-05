using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        Stream stream;
        static void Main(string[] args)
        {
            new Server();
        }

        public Server()
        {
            TcpListener server = null;

            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data


                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    stream = client.GetStream();

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

                    clientThread.Start(client);


                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }



            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private void HandleClientComm(object client)
        {
            int i;
            Byte[] bytes = new Byte[256];
            string data = null;
            // Process the data sent by the client.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                //SaveData(JsonConvert.SerializeObject(data));

                byte[] msg = System.Text.Encoding.ASCII.GetBytes($"Received: {data}");

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", "Acknowledged");
            }
            // Shutdown and end connection

        }




        public JObject ReadObject()
        {
            byte[] preBuffer = new Byte[4];
            stream.Read(preBuffer, 0, 4);
            int lenght = BitConverter.ToInt32(preBuffer, 0);
            byte[] buffer = new Byte[lenght];
            int totalReceived = 0;
            while (totalReceived < lenght)
            {
                int receivedCount = stream.Read(buffer, totalReceived, lenght - totalReceived);
                totalReceived += receivedCount;
            }
            JObject Json = JObject.Parse(Encoding.UTF8.GetString(buffer));
            Console.WriteLine(Json);
            return Json;
        }

        public void SaveData(string message)
        {
            using (StreamWriter file = File.CreateText("data.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, message);
            }
        }

        public dynamic LoadData()
        {
            using (StreamReader file = File.OpenText("data.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(file, typeof(string));
            }
        }
    }
}

