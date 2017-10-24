#if ANDROID || IOS
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
using Color = Microsoft.Xna.Framework.Color;
using tankgame.Screens;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using tankgame.Entities;
using tankgame.Factories;
using FlatRedBall;
using FlatRedBall.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.Math.Geometry;
namespace tankgame.Entities
{
    public partial class Tank2 : FlatRedBall.PositionedObject, FlatRedBall.Graphics.IDestroyable, FlatRedBall.Math.Geometry.ICollidable
    {
        // This is made static so that static lazy-loaded content can access it.
        public static string ContentManagerName { get; set; }
        #if DEBUG
        static bool HasBeenLoadedWithGlobalContentManager = false;
        #endif
        static object mLockObject = new object();
        static System.Collections.Generic.List<string> mRegisteredUnloads = new System.Collections.Generic.List<string>();
        static System.Collections.Generic.List<string> LoadedContentManagers = new System.Collections.Generic.List<string>();
        protected static Microsoft.Xna.Framework.Graphics.Texture2D PZiv;
        
        private FlatRedBall.Sprite Sprite2;
        private FlatRedBall.Math.Geometry.Polygon mHitbox2;
        public FlatRedBall.Math.Geometry.Polygon Hitbox2
        {
            get
            {
                return mHitbox2;
            }
            private set
            {
                mHitbox2 = value;
            }
        }
        public float MovementSpeed = 1f;
        public float TurningSpeed;
        private FlatRedBall.Math.Geometry.ShapeCollection mGeneratedCollision;
        public FlatRedBall.Math.Geometry.ShapeCollection Collision
        {
            get
            {
                return mGeneratedCollision;
            }
        }
        protected FlatRedBall.Graphics.Layer LayerProvidedByContainer = null;
        public Tank2 ()
        	: this(FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName, true)
        {
        }
        public Tank2 (string contentManagerName)
        	: this(contentManagerName, true)
        {
        }
        public Tank2 (string contentManagerName, bool addToManagers)
        	: base()
        {
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);
        }
        protected virtual void InitializeEntity (bool addToManagers)
        {
            LoadStaticContent(ContentManagerName);
            Sprite2 = new FlatRedBall.Sprite();
            Sprite2.Name = "Sprite2";
            mHitbox2 = new FlatRedBall.Math.Geometry.Polygon();
            mHitbox2.Name = "mHitbox2";
            
            PostInitialize();
            if (addToManagers)
            {
                AddToManagers(null);
            }
        }
        public virtual void ReAddToManagers (FlatRedBall.Graphics.Layer layerToAddTo)
        {
            LayerProvidedByContainer = layerToAddTo;
            FlatRedBall.SpriteManager.AddPositionedObject(this);
            FlatRedBall.SpriteManager.AddToLayer(Sprite2, LayerProvidedByContainer);
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(mHitbox2, LayerProvidedByContainer);
        }
        public virtual void AddToManagers (FlatRedBall.Graphics.Layer layerToAddTo)
        {
            LayerProvidedByContainer = layerToAddTo;
            FlatRedBall.SpriteManager.AddPositionedObject(this);
            FlatRedBall.SpriteManager.AddToLayer(Sprite2, LayerProvidedByContainer);
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(mHitbox2, LayerProvidedByContainer);
            AddToManagersBottomUp(layerToAddTo);
            CustomInitialize();
        }
        public virtual void Activity ()
        {
            
            CustomActivity();
        }
        public virtual void Destroy ()
        {
            FlatRedBall.SpriteManager.RemovePositionedObject(this);
            
            if (Sprite2 != null)
            {
                FlatRedBall.SpriteManager.RemoveSprite(Sprite2);
            }
            if (Hitbox2 != null)
            {
                FlatRedBall.Math.Geometry.ShapeManager.Remove(Hitbox2);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
            CustomDestroy();
        }
        public virtual void PostInitialize ()
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            if (Sprite2.Parent == null)
            {
                Sprite2.CopyAbsoluteToRelative();
                Sprite2.AttachTo(this, false);
            }
            Sprite2.Texture = PZiv;
            Sprite2.FlipHorizontal = false;
            Sprite2.FlipVertical = true;
            Sprite2.TextureScale = 0.3f;
            Sprite2.Visible = true;
            if (mHitbox2.Parent == null)
            {
                mHitbox2.CopyAbsoluteToRelative();
                mHitbox2.AttachTo(this, false);
            }
            Hitbox2.Visible = true;
            FlatRedBall.Math.Geometry.Point[] Hitbox2Points = new FlatRedBall.Math.Geometry.Point[] { new FlatRedBall.Math.Geometry.Point(0, 16), new FlatRedBall.Math.Geometry.Point(16, 0), new FlatRedBall.Math.Geometry.Point(-16, 0),  new FlatRedBall.Math.Geometry.Point(0, 16) };
            Hitbox2.Points = Hitbox2Points;
            mGeneratedCollision = new FlatRedBall.Math.Geometry.ShapeCollection();
            mGeneratedCollision.Polygons.AddOneWay(mHitbox2);
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public virtual void AddToManagersBottomUp (FlatRedBall.Graphics.Layer layerToAddTo)
        {
            AssignCustomVariables(false);
        }
        public virtual void RemoveFromManagers ()
        {
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
            if (Sprite2 != null)
            {
                FlatRedBall.SpriteManager.RemoveSpriteOneWay(Sprite2);
            }
            if (Hitbox2 != null)
            {
                FlatRedBall.Math.Geometry.ShapeManager.RemoveOneWay(Hitbox2);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
        }
        public virtual void AssignCustomVariables (bool callOnContainedElements)
        {
            if (callOnContainedElements)
            {
            }
            Sprite2.Texture = PZiv;
            Sprite2.FlipHorizontal = false;
            Sprite2.FlipVertical = true;
            Sprite2.TextureScale = 0.3f;
            Sprite2.Visible = true;
            Hitbox2.Visible = true;
            Drag = 1f;
            MovementSpeed = 1f;
        }
        public virtual void ConvertToManuallyUpdated ()
        {
            this.ForceUpdateDependenciesDeep();
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(Sprite2);
        }
        public static void LoadStaticContent (string contentManagerName)
        {
            if (string.IsNullOrEmpty(contentManagerName))
            {
                throw new System.ArgumentException("contentManagerName cannot be empty or null");
            }
            ContentManagerName = contentManagerName;
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
            bool registerUnload = false;
            if (LoadedContentManagers.Contains(contentManagerName) == false)
            {
                LoadedContentManagers.Add(contentManagerName);
                lock (mLockObject)
                {
                    if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
                    {
                        FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("Tank2StaticUnload", UnloadStaticContent);
                        mRegisteredUnloads.Add(ContentManagerName);
                    }
                }
                if (!FlatRedBall.FlatRedBallServices.IsLoaded<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/entities/tank/pziv.png", ContentManagerName))
                {
                    registerUnload = true;
                }
                PZiv = FlatRedBall.FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/entities/tank/pziv.png", ContentManagerName);
            }
            if (registerUnload && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
            {
                lock (mLockObject)
                {
                    if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBall.FlatRedBallServices.GlobalContentManager)
                    {
                        FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("Tank2StaticUnload", UnloadStaticContent);
                        mRegisteredUnloads.Add(ContentManagerName);
                    }
                }
            }
            CustomLoadStaticContent(contentManagerName);
        }
        public static void UnloadStaticContent ()
        {
            if (LoadedContentManagers.Count != 0)
            {
                LoadedContentManagers.RemoveAt(0);
                mRegisteredUnloads.RemoveAt(0);
            }
            if (LoadedContentManagers.Count == 0)
            {
                if (PZiv != null)
                {
                    PZiv= null;
                }
            }
        }
        [System.Obsolete("Use GetFile instead")]
        public static object GetStaticMember (string memberName)
        {
            switch(memberName)
            {
                case  "PZiv":
                    return PZiv;
            }
            return null;
        }
        public static object GetFile (string memberName)
        {
            switch(memberName)
            {
                case  "PZiv":
                    return PZiv;
            }
            return null;
        }
        object GetMember (string memberName)
        {
            switch(memberName)
            {
                case  "PZiv":
                    return PZiv;
            }
            return null;
        }
        protected bool mIsPaused;
        public override void Pause (FlatRedBall.Instructions.InstructionList instructions)
        {
            base.Pause(instructions);
            mIsPaused = true;
        }
        public virtual void SetToIgnorePausing ()
        {
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(this);
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(Sprite2);
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(Hitbox2);
        }
        public virtual void MoveToLayer (FlatRedBall.Graphics.Layer layerToMoveTo)
        {
            var layerToRemoveFrom = LayerProvidedByContainer;
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(Sprite2);
            }
            FlatRedBall.SpriteManager.AddToLayer(Sprite2, layerToMoveTo);
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(Hitbox2);
            }
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(Hitbox2, layerToMoveTo);
            LayerProvidedByContainer = layerToMoveTo;
        }
    }
}
