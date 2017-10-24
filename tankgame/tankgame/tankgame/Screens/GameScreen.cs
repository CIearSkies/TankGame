using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.TileCollisions;
using tankgame.Entities;
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
        private int hitpointsTank1;
        private int hitpointsTank2;
        FlatRedBall.TileCollisions.TileShapeCollection wallCollision;
        void CustomInitialize()
		{
            hitpointsTank1 = 200;
            hitpointsTank2 = 200;

            Camera.Main.X = Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y = -1 * Camera.Main.OrthogonalHeight / 2.0f;
            wallCollision = new FlatRedBall.TileCollisions.TileShapeCollection();
            wallCollision.Visible = false;
            wallCollision.AddCollisionFrom(woods, new List<String> { "Tree1", "Tree3", "Tree2", "Tree4", "Tree5", "Tree6", "Plant",
                "Log1", "Log2", "Log3","Log4", "Log5", "Log6", "Log7", "Log8", "Grass", "InvisiWall",
                "Water1", "Water2", "Water3", "Water4", "Water5", "Water6", "Water7", "Water8", "Water9", "Water10",
                "Water11", "Water12", "Water13", "Water14", "Water15", "Water16", "Water17", "Water18", "Water19", "Water20",
                "Water21", "Water22", "Water23", "Water24", "Water25", "Water26", "Water27", "Water28", "Water29", "Water30",
                "Water31", "Water32", "Water33", "Water34", "Water35", "Water36", "Water37", "Water38", "Water39", "Water40",
                "Water41", "Water42",
                "Stone1", "Stone2"});
            
        }

		void CustomActivity(bool firstTimeCalled)
		{
            
            wallCollision.CollideAgainstSolid(Player1Tank);
            wallCollision.CollideAgainstSolid(Player2Tank);
            BulletCollision();
        }

        void BulletCollision()
        {
            for (int i = BulletList.Count - 1; i > -1; i--)
            {
                Bullet bullet = BulletList[i];
                if (wallCollision.CollideAgainstSolid(bullet.CircleInstance))
                {
                    //FlatRedBall.Debugging.Debugger.Write("HIT");
                    bullet.Destroy();
                    break;
                }

                if (bullet.CircleInstance.CollideAgainst(Player1Tank.Hitbox))
                {
                    bullet.Destroy();
                    hitpointsTank1 = hitpointsTank1 - 50;
                    if (hitpointsTank1 == 0)
                    {
                        Player1Tank.Destroy();
                    }
                }
                if (bullet.CircleInstance.CollideAgainst(Player2Tank.Hitbox2))
                {
                    bullet.Destroy();
                    hitpointsTank2 = hitpointsTank2 - 50;
                    if (hitpointsTank2 == 0)
                    {
                        Player2Tank.Destroy();
                    }
                }
            }
        }

		void CustomDestroy()
		{
            wallCollision.RemoveFromManagers();
        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
