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
using Newtonsoft.Json;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace tankgame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        NetworkStream stream;

        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public string Player { get; set; }

        public Game1() : base ()
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
            BinaryFormatter formatter = new BinaryFormatter();
            if (Player == "player1")
            {
                    formatter.Serialize(stream, Position);
                formatter.Serialize(stream, Rotation);
               

            } else if (Player == "player2")
            {
                Position = (Vector3) formatter.Deserialize(stream);
                Rotation = (float) formatter.Deserialize(stream);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            FlatRedBallServices.Draw();

            base.Draw(gameTime);
        }
    }
}
