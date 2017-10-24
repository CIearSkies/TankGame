using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using FlatRedBall.Math.Geometry;
using tankgame.Factories;
using System.Threading;
using System.Timers;

namespace tankgame.Entities
{
    public partial class Tank2
    {
        Boolean shoot = true;
        System.Timers.Timer timer;
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

            buttonMap.A = Keys.Space;
            buttonMap.B = Keys.S;
            buttonMap.X = Keys.Q;
            buttonMap.Y = Keys.W;

            var points2 = new List<FlatRedBall.Math.Geometry.Point>
            {
                  new Point(-17, 34.5),
                  new Point(17, 34.5),
                  new Point(17, -34.5),
                  new Point(-17, -34.5),
                  new Point(-17, 34.5)
                  // repeat the last point to close the shape2
                  
            };
            Hitbox2.Points = points2;
            
            InputManager.Xbox360GamePads[0].ButtonMap = buttonMap;
            makeTimer();
        }

        public void Handler(object sender, ElapsedEventArgs e)
        {

            shoot = true;
        }
        private void CustomActivity()
        {
            MovementActivity();
            TurningActivity();
            ShootingActivity();
            if (game.Player == "player2")
            {
                GivePosition();
                GiveRotation();
            }
            else if (game.Player == "player1")
            {
                UpdatePosition();
                UpdateRotation();
                UpdateBullet();
            }
        }

        public void makeTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer(4000);
            timer.Elapsed += Handler;
            timer.Start();
        }

        private void ShootingActivity()
        {
            if (mGamePad.ButtonPushed(Xbox360GamePad.Button.A) && shoot)
            {
                // We'll create 2 bullets because it looks much cooler than 1
                Bullet firstBullet = BulletFactory.CreateNew();
                firstBullet.Position = this.Position;
                firstBullet.Position += this.RotationMatrix.Up * 12;
                // This is the bullet on the right side when the ship is facing up.
                // Adding along the Right vector will move it to the right relative to the ship
                firstBullet.Position += this.RotationMatrix.Right * 6;
                firstBullet.RotationZ = this.RotationZ;
                firstBullet.Velocity = this.RotationMatrix.Up * firstBullet.MovementSpeed;
                GiveBullet();
                shoot = false;
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
                this.Acceleration = -this.RotationMatrix.Up * 100;
            else { this.Acceleration -= this.Acceleration; }

            if (mGamePad.LeftStick.Position.Y == -1)
                this.Acceleration = -this.RotationMatrix.Up * 100;
            else { this.Acceleration = -this.Acceleration; }
        }

        void TurningActivity()
        {
            this.RotationZVelocity = -mGamePad.LeftStick.Position.X * 0.7f;
        }

        void GivePosition()
        {

            game.Position2 = this.Position;
        }

        void GiveRotation()
        {
            game.Rotation2 = this.RotationZ;
        }

        void UpdatePosition()
        {
            this.Position = game.Position2;
        }

        void UpdateRotation()
        {
            this.RotationZ = game.Rotation2;
        }

        void GiveBullet()
        {
            game.Shoot2 = this.shoot;
        }

        void UpdateBullet()
        {
            this.shoot = game.Shoot2;
            if (this.shoot)
            {
                Bullet firstBullet = BulletFactory.CreateNew();
                firstBullet.Position = this.Position;
                firstBullet.Position += this.RotationMatrix.Up * 12;
                // This is the bullet on the right side when the ship is facing up.
                // Adding along the Right vector will move it to the right relative to the ship
                firstBullet.Position += this.RotationMatrix.Right * 6;
                firstBullet.RotationZ = this.RotationZ;
                firstBullet.Velocity = this.RotationMatrix.Up * firstBullet.MovementSpeed;
                shoot = false;
            }
        }
    }
}

