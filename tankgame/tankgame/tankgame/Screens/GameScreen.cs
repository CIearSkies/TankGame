using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;



namespace tankgame.Screens
{
	public partial class GameScreen
	{

		void CustomInitialize()
		{
            Camera.Main.X = Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y = -1 * Camera.Main.OrthogonalHeight / 2.0f;
        }

		void CustomActivity(bool firstTimeCalled)
		{


		}

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
