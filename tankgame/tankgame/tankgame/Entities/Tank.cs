using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework.Content;

namespace tankgame.Entities
{
    public partial class Tank
    {
        Game1 game;
        Xbox360GamePad mGamePad;
        private void CustomInitialize()
        {
            game = (Game1)FlatRedBallServices.Game;
            mGamePad = InputManager.Xbox360GamePads[0];
            KeyboardButtonMap buttonMap = new KeyboardButtonMap();

            buttonMap.LeftAnalogLeft = Keys.Left;
            buttonMap.LeftAnalogRight = Keys.Right;
            buttonMap.LeftAnalogUp = Keys.Up;
            buttonMap.LeftAnalogDown = Keys.Down;

            buttonMap.A = Keys.A;
            buttonMap.B = Keys.S;
            buttonMap.X = Keys.Q;
            buttonMap.Y = Keys.W;

            var points = new List<FlatRedBall.Math.Geometry.Point>
            {
                  new Point(-17, 34.5),
                  new Point(17, 34.5),
                  new Point(17, -34.5),
                  new Point(-17, -34.5),
                  new Point(-17, 34.5)
                  // repeat the last point to close the shape
                  
            };
            Hitbox.Points = points;

            InputManager.Xbox360GamePads[0].ButtonMap = buttonMap;
        }

        private void CustomActivity()
        {
            MovementActivity();
            TurningActivity();
            if (game.Player == "player1")
            {
                GivePosition();
                GiveRotation();
            }
            else if (game.Player == "player2")
            {
                UpdatePosition();
                UpdateRotation();
            }
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        void MovementActivity()
        {
            if (mGamePad.LeftStick.Position.Y == 1)
                this.Acceleration = -this.RotationMatrix.Up * MovementSpeed;
            else { this.Acceleration -= this.Acceleration; }

            if (mGamePad.LeftStick.Position.Y == -1)
                this.Acceleration = -this.RotationMatrix.Up * MovementSpeed;
            else { this.Acceleration = -this.Acceleration; }
        }

        void TurningActivity()
        {
            this.RotationZVelocity = -mGamePad.LeftStick.Position.X * 0.7f;
        }

        void GivePosition()
        {
            
            game.Position = this.Position;
        }

        void GiveRotation()
        {
            game.Rotation = this.RotationZ;
        }

        void UpdatePosition()
        {
            this.Position = game.Position;
        }

        void UpdateRotation()
        {
            this.RotationZ = game.Rotation;
        }
    }
}
