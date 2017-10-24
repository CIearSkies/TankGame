using System;
using System.Collections.Generic;
using System.Reflection;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;

using System.Linq;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Threading;

namespace tankgame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        NetworkStream stream;

        public Vector3 Position1 { get; set; }
        public float Rotation1 { get; set; }
        public string Player { get; set; }
        public bool Shoot1 { get; set; }
        public Vector3 Position2 { get; set; }
        public float Rotation2 { get; set; }
        public bool Shoot2 { get; set; }
        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);

#if WINDOWS_PHONE || ANDROID || IOS

			// Frame rate is 30 fps by default for Windows Phone,
            // so let's keep that for other phones too
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            graphics.IsFullScreen = true;
#elif WINDOWS || DESKTOP_GL
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
#endif


#if WINDOWS_8
            FlatRedBall.Instructions.Reflection.PropertyValuePair.TopLevelAssembly = 
                this.GetType().GetTypeInfo().Assembly;
#endif

        }

        protected override void Initialize()
        {
#if IOS
			var bounds = UIKit.UIScreen.MainScreen.Bounds;
			var nativeScale = UIKit.UIScreen.MainScreen.Scale;
			var screenWidth = (int)(bounds.Width * nativeScale);
			var screenHeight = (int)(bounds.Height * nativeScale);
			graphics.PreferredBackBufferWidth = screenWidth;
			graphics.PreferredBackBufferHeight = screenHeight;
#endif

            try
            {

                Int32 port = 13000;
                TcpClient client = new TcpClient("127.0.0.1", port);

                stream = client.GetStream();

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

                if (Json.GetValue("id").ToString() == "player1")
                {
                    Player = "player1";
                }
                else if (Json.GetValue("id").ToString() == "player2")
                {
                    Player = "player2";
                }
                preBuffer = new Byte[4];
                stream.Read(preBuffer, 0, 4);
                lenght = BitConverter.ToInt32(preBuffer, 0);
                buffer = new Byte[lenght];
                totalReceived = 0;
                while (totalReceived < lenght)
                {
                    int receivedCount = stream.Read(buffer, totalReceived, lenght - totalReceived);
                    totalReceived += receivedCount;
                }
                Json = JObject.Parse(Encoding.UTF8.GetString(buffer));

                if (Json.GetValue("id").ToString() == "start")
                {
                    FlatRedBallServices.InitializeFlatRedBall(this, graphics);
                    FlatRedBallServices.GraphicsOptions.TextureFilter = TextureFilter.Point;
                    CameraSetup.SetupCamera(SpriteManager.Camera, graphics);
			GlobalContent.Initialize();
			FlatRedBall.Screens.ScreenManager.Start(typeof(tankgame.Screens.GameScreen));
                    base.Initialize();
                    Thread update = new Thread(updater);
                    update.Start();
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }


        protected override void Update(GameTime gameTime)
        {
            FlatRedBallServices.Update(gameTime);

            FlatRedBall.Screens.ScreenManager.Activity();

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            FlatRedBallServices.Draw();

            base.Draw(gameTime);


        }

        public JObject ReadObject(NetworkStream stream)
        {
            try
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
                Console.WriteLine(Encoding.UTF8.GetString(buffer));
                JObject Json = JObject.Parse(Encoding.UTF8.GetString(buffer));
                Console.WriteLine(Json);
                return Json;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
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

        public void updater()
        {
            while (true)
            {
                if (Player == "player1")
                {
                    dynamic toJson = new
                    {
                        id = "player1/position",
                        data = new
                        {
                            position = Position1
                        }
                    };
                    SendObject(JsonConvert.SerializeObject(toJson), stream);
                    JObject jsondata = ReadObject(stream);
                    if (jsondata.GetValue("id").ToString() == "player2/position")
                    {
                        Position2 = jsondata.SelectToken("data").SelectToken("position2").ToObject<Vector3>();
                    }
                    toJson = new
                    {
                        id = "player1/rotation",
                        data = new
                        {
                            rotation = Rotation1
                        }
                    };
                    SendObject(JsonConvert.SerializeObject(toJson), stream);
                    jsondata = ReadObject(stream);
                    if (jsondata.GetValue("id").ToString() == "player2/rotation")
                    {
                        Rotation2 = jsondata.SelectToken("data").SelectToken("rotation2").ToObject<float>();
                    }
                    toJson = new
                    {
                        id = "player1/shoot",
                        data = new
                        {
                            shoot = Shoot1
                        }
                    };

                    SendObject(JsonConvert.SerializeObject(toJson), stream);
                    jsondata = ReadObject(stream);
                    if (jsondata.GetValue("id").ToString() == "player2/shoot")
                    {
                        Shoot2 = jsondata.SelectToken("data").SelectToken("shoot2").ToObject<bool>();
                    }
                    Shoot1 = false;

                }
                else if (Player == "player2")
                {
                    dynamic toJson = new
                    {
                        id = "player2/position",
                        data = new
                        {
                            position2 = Position2
                        }
                    };
                    SendObject(JsonConvert.SerializeObject(toJson), stream);
                    JObject jsondata = ReadObject(stream);
                    if (jsondata.GetValue("id").ToString() == "player1/position")
                    {
                        Position1 = jsondata.SelectToken("data").SelectToken("position").ToObject<Vector3>();
                    }
                    toJson = new
                    {
                        id = "player2/rotation",
                        data = new
                        {
                            rotation2 = Rotation2
                        }
                    };
                    SendObject(JsonConvert.SerializeObject(toJson), stream);
                    jsondata = ReadObject(stream);
                    if (jsondata.GetValue("id").ToString() == "player1/rotation")
                    {
                        Rotation1 = jsondata.SelectToken("data").SelectToken("rotation").ToObject<float>();
                    }
                    toJson = new
                    {
                        id = "player2/shoot",
                        data = new
                        {
                            shoot2 = Shoot2
                        }
                    };
                    SendObject(JsonConvert.SerializeObject(toJson), stream);
                    jsondata = ReadObject(stream);
                    if (jsondata.GetValue("id").ToString() == "player1/shoot")
                    {
                        Shoot1 = jsondata.SelectToken("data").SelectToken("shoot").ToObject<bool>();
                    }

                    Shoot2 = false;
                }
            }

        }
    }
}
