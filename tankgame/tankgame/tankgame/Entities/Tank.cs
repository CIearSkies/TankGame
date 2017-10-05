using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework.Input;

namespace tankgame.Entities
{
	public partial class Tank
	{
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        Xbox360GamePad mGamePad;
        private void CustomInitialize()
		{
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

            InputManager.Xbox360GamePads[0].ButtonMap = buttonMap;
        }

		private void CustomActivity()
		{
            MovementActivity();
            TurningActivity();
        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        void MovementActivity()
        {
            if(mGamePad.LeftStick.Position.Y == 1)
             this.Acceleration = -this.RotationMatrix.Up * this.MovementSpeed;
            else { this.Acceleration -= this.Acceleration; }
        }

        void TurningActivity()
        {
            // We use a negative value because we want holding left to turn
            // counterclockwise - but left is a negative value and turning counterclockwise 
            // is a positive value
            this.RotationZVelocity = -mGamePad.LeftStick.Position.X * TurningSpeed;
        }
    }
}
