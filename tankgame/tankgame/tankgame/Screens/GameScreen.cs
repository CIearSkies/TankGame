using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.TileCollisions;
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
        FlatRedBall.TileCollisions.TileShapeCollection wallCollision;
        void CustomInitialize()
		{
            Camera.Main.X = Camera.Main.OrthogonalWidth / 2.0f;
            Camera.Main.Y = -1 * Camera.Main.OrthogonalHeight / 2.0f;
            wallCollision = new FlatRedBall.TileCollisions.TileShapeCollection();
            //wallCollision.Visible = true;
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
