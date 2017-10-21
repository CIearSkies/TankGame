#if ANDROID || IOS
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
using Color = Microsoft.Xna.Framework.Color;
using tankgame.Entities;
using FlatRedBall;
using FlatRedBall.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.Math;
namespace tankgame.Screens
{
    public partial class GameScreen : FlatRedBall.Screens.Screen
    {
        #if DEBUG
        static bool HasBeenLoadedWithGlobalContentManager = false;
        #endif
        protected static Microsoft.Xna.Framework.Graphics.Texture2D forest_tiles;
        protected static FlatRedBall.TileGraphics.LayeredTileMap woods;
        
        private FlatRedBall.Math.PositionedObjectList<tankgame.Entities.Tank> TankList;
        public tankgame.Entities.Tank Player1Tank;
        private FlatRedBall.Math.PositionedObjectList<tankgame.Entities.Wall> WallList;
        private FlatRedBall.Math.PositionedObjectList<tankgame.Entities.Bullet> BulletList;
        public GameScreen ()
        	: base ("GameScreen")
        {
        }
        public override void Initialize (bool addToManagers)
        {
            LoadStaticContent(ContentManagerName);
            TankList = new FlatRedBall.Math.PositionedObjectList<tankgame.Entities.Tank>();
            TankList.Name = "TankList";
            Player1Tank = new tankgame.Entities.Tank(ContentManagerName, false);
            Player1Tank.Name = "Player1Tank";
            WallList = new FlatRedBall.Math.PositionedObjectList<tankgame.Entities.Wall>();
            WallList.Name = "WallList";
            BulletList = new FlatRedBall.Math.PositionedObjectList<tankgame.Entities.Bullet>();
            BulletList.Name = "BulletList";
            
            
            PostInitialize();
            base.Initialize(addToManagers);
            if (addToManagers)
            {
                AddToManagers();
            }
        }
        public override void AddToManagers ()
        {
            woods.AddToManagers(mLayer);
            Player1Tank.AddToManagers(mLayer);
            base.AddToManagers();
            AddToManagersBottomUp();
            CustomInitialize();
        }
        public override void Activity (bool firstTimeCalled)
        {
            if (!IsPaused)
            {
                
                for (int i = TankList.Count - 1; i > -1; i--)
                {
                    if (i < TankList.Count)
                    {
                        // We do the extra if-check because activity could destroy any number of entities
                        TankList[i].Activity();
                    }
                }
                for (int i = WallList.Count - 1; i > -1; i--)
                {
                    if (i < WallList.Count)
                    {
                        // We do the extra if-check because activity could destroy any number of entities
                        WallList[i].Activity();
                    }
                }
                for (int i = BulletList.Count - 1; i > -1; i--)
                {
                    if (i < BulletList.Count)
                    {
                        // We do the extra if-check because activity could destroy any number of entities
                        BulletList[i].Activity();
                    }
                }
            }
            else
            {
            }
            base.Activity(firstTimeCalled);
            if (!IsActivityFinished)
            {
                CustomActivity(firstTimeCalled);
            }
        }
        public override void Destroy ()
        {
            base.Destroy();
            forest_tiles = null;
            woods.Destroy();
            woods = null;
            
            TankList.MakeOneWay();
            WallList.MakeOneWay();
            BulletList.MakeOneWay();
            for (int i = TankList.Count - 1; i > -1; i--)
            {
                TankList[i].Destroy();
            }
            for (int i = WallList.Count - 1; i > -1; i--)
            {
                WallList[i].Destroy();
            }
            for (int i = BulletList.Count - 1; i > -1; i--)
            {
                BulletList[i].Destroy();
            }
            TankList.MakeTwoWay();
            WallList.MakeTwoWay();
            BulletList.MakeTwoWay();
            CustomDestroy();
        }
        public virtual void PostInitialize ()
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            TankList.Add(Player1Tank);
            if (Player1Tank.Parent == null)
            {
                Player1Tank.X = 400f;
            }
            else
            {
                Player1Tank.RelativeX = 400f;
            }
            if (Player1Tank.Parent == null)
            {
                Player1Tank.Y = -400f;
            }
            else
            {
                Player1Tank.RelativeY = -400f;
            }
            if (Player1Tank.Parent == null)
            {
                Player1Tank.Z = 10f;
            }
            else
            {
                Player1Tank.RelativeZ = 10f;
            }
            Player1Tank.Drag = 1f;
            Player1Tank.MovementSpeed = 350f;
            Player1Tank.TurningSpeed = 2f;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public virtual void AddToManagersBottomUp ()
        {
            CameraSetup.ResetCamera(SpriteManager.Camera);
            AssignCustomVariables(false);
        }
        public virtual void RemoveFromManagers ()
        {
            for (int i = TankList.Count - 1; i > -1; i--)
            {
                TankList[i].Destroy();
            }
            for (int i = WallList.Count - 1; i > -1; i--)
            {
                WallList[i].Destroy();
            }
            for (int i = BulletList.Count - 1; i > -1; i--)
            {
                BulletList[i].Destroy();
            }
        }
        public virtual void AssignCustomVariables (bool callOnContainedElements)
        {
            if (callOnContainedElements)
            {
                Player1Tank.AssignCustomVariables(true);
            }
            if (Player1Tank.Parent == null)
            {
                Player1Tank.X = 400f;
            }
            else
            {
                Player1Tank.RelativeX = 400f;
            }
            if (Player1Tank.Parent == null)
            {
                Player1Tank.Y = -400f;
            }
            else
            {
                Player1Tank.RelativeY = -400f;
            }
            if (Player1Tank.Parent == null)
            {
                Player1Tank.Z = 10f;
            }
            else
            {
                Player1Tank.RelativeZ = 10f;
            }
            Player1Tank.Drag = 1f;
            Player1Tank.MovementSpeed = 350f;
            Player1Tank.TurningSpeed = 2f;
        }
        public virtual void ConvertToManuallyUpdated ()
        {
            for (int i = 0; i < TankList.Count; i++)
            {
                TankList[i].ConvertToManuallyUpdated();
            }
            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].ConvertToManuallyUpdated();
            }
            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].ConvertToManuallyUpdated();
            }
        }
        public static void LoadStaticContent (string contentManagerName)
        {
            if (string.IsNullOrEmpty(contentManagerName))
            {
                throw new System.ArgumentException("contentManagerName cannot be empty or null");
            }
            #if DEBUG
            if (contentManagerName == FlatRedBall.FlatRedBallServices.GlobalContentManager)
            {
                HasBeenLoadedWithGlobalContentManager = true;
            }
            else if (HasBeenLoadedWithGlobalContentManager)
            {
                throw new System.Exception("This type has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
            }
            #endif
            forest_tiles = FlatRedBall.FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/screens/gamescreen/forest_tiles.png", contentManagerName);
            woods = FlatRedBall.TileGraphics.LayeredTileMap.FromTiledMapSave("content/screens/gamescreen/woods.tmx", contentManagerName);
            CustomLoadStaticContent(contentManagerName);
        }
        [System.Obsolete("Use GetFile instead")]
        public static object GetStaticMember (string memberName)
        {
            switch(memberName)
            {
                case  "forest_tiles":
                    return forest_tiles;
                case  "woods":
                    return woods;
            }
            return null;
        }
        public static object GetFile (string memberName)
        {
            switch(memberName)
            {
                case  "forest_tiles":
                    return forest_tiles;
                case  "woods":
                    return woods;
            }
            return null;
        }
        object GetMember (string memberName)
        {
            switch(memberName)
            {
                case  "forest_tiles":
                    return forest_tiles;
                case  "woods":
                    return woods;
            }
            return null;
        }
    }
}
