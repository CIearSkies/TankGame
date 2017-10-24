using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        JObject jsondata;
        NetworkStream stream;
        NetworkStream player1stream;
        NetworkStream player2stream;
        bool player1exists = false;
        byte[] buffer;
        byte[] request;
        byte[] prefix;
        string message;
        dynamic toJson;
        bool klaar;
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
                IPAddress localAddr = IPAddress.Parse("192.168.2.188");

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

                    if (!player1exists)
                    {
                        Console.WriteLine("player1");

                        dynamic toJson = new
                        {
                            id = "player1",
                            data = new
                            {
                            }
                        };
                        string message = JsonConvert.SerializeObject(toJson);

                        byte[] prefix = BitConverter.GetBytes(message.Length);
                        byte[] request = Encoding.Default.GetBytes(message);

                        byte[] buffer = new Byte[prefix.Length + message.Length];
                        prefix.CopyTo(buffer, 0);
                        request.CopyTo(buffer, prefix.Length);

                        stream.Write(buffer, 0, buffer.Length);

                        player1stream = stream;

                        Thread player1Thread = new Thread(HandlePlayer1Comm);

                        player1Thread.Start(client);

                        player1exists = true;
                    }
                    else if (player1exists)
                    {
                        Console.WriteLine("player2");

                        dynamic toJson = new
                        {
                            id = "player2",
                            data = new
                            {
                            }
                        };
                        string message = JsonConvert.SerializeObject(toJson);

                        byte[] prefix = BitConverter.GetBytes(message.Length);
                        byte[] request = Encoding.Default.GetBytes(message);

                        byte[] buffer = new Byte[prefix.Length + message.Length];
                        prefix.CopyTo(buffer, 0);
                        request.CopyTo(buffer, prefix.Length);

                        stream.Write(buffer, 0, buffer.Length);
                        

                        player2stream = stream;

                        Thread player2Thread = new Thread(HandlePlayer2Comm);

                        player2Thread.Start(client);
                        klaar = true;
                    }


                    

                    if (klaar) {

                        dynamic start = new
                        {
                            id = "start",
                            data = new
                            {
                            }
                        };
                        message = JsonConvert.SerializeObject(start);

                        prefix = BitConverter.GetBytes(message.Length);
                        request = Encoding.Default.GetBytes(message);

                        buffer = new Byte[prefix.Length + message.Length];
                        prefix.CopyTo(buffer, 0);
                        request.CopyTo(buffer, prefix.Length);

                        player1stream.Write(buffer, 0, buffer.Length);
                        player2stream.Write(buffer, 0, buffer.Length);
                        break;
                    }
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
           
        }

        private void HandlePlayer1Comm(object client)
        {
            while (true)
            {
                jsondata = ReadObject(player1stream);
                Console.WriteLine(jsondata);
                SendObject(JsonConvert.SerializeObject(jsondata), player2stream);
            }
        }


        private void HandlePlayer2Comm(object client)
        {
            while (true)
            {
                jsondata = ReadObject(player2stream);
                Console.WriteLine(jsondata);
                SendObject(JsonConvert.SerializeObject(jsondata), player1stream);
            }
        }
        




        public JObject ReadObject(NetworkStream stream)
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
            //Console.WriteLine(Encoding.UTF8.GetString(buffer));
            JObject Json = JObject.Parse(Encoding.UTF8.GetString(buffer));
            
           // Console.WriteLine(Json);
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
        public void SendObject(string message, NetworkStream stream)
        {
            try
            {
                byte[] prefix = BitConverter.GetBytes(message.Length);
                byte[] request = Encoding.Default.GetBytes(message);

                byte[] buffer = new Byte[prefix.Length + message.Length];
                prefix.CopyTo(buffer, 0);
                request.CopyTo(buffer, prefix.Length);

                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

