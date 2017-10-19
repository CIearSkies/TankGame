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
    public partial class Tank : FlatRedBall.PositionedObject, FlatRedBall.Graphics.IDestroyable, FlatRedBall.Math.Geometry.ICollidable
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
        
        private FlatRedBall.Sprite Sprite;
        private FlatRedBall.Math.Geometry.Polygon mHitbox;
        public FlatRedBall.Math.Geometry.Polygon Hitbox
        {
            get
            {
                return mHitbox;
            }
            private set
            {
                mHitbox = value;
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
        public Tank ()
        	: this(FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName, true)
        {
        }
        public Tank (string contentManagerName)
        	: this(contentManagerName, true)
        {
        }
        public Tank (string contentManagerName, bool addToManagers)
        	: base()
        {
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);
        }
        protected virtual void InitializeEntity (bool addToManagers)
        {
            LoadStaticContent(ContentManagerName);
            Sprite = new FlatRedBall.Sprite();
            Sprite.Name = "Sprite";
            mHitbox = new FlatRedBall.Math.Geometry.Polygon();
            mHitbox.Name = "mHitbox";
            
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
            FlatRedBall.SpriteManager.AddToLayer(Sprite, LayerProvidedByContainer);
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(mHitbox, LayerProvidedByContainer);
        }
        public virtual void AddToManagers (FlatRedBall.Graphics.Layer layerToAddTo)
        {
            LayerProvidedByContainer = layerToAddTo;
            FlatRedBall.SpriteManager.AddPositionedObject(this);
            FlatRedBall.SpriteManager.AddToLayer(Sprite, LayerProvidedByContainer);
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(mHitbox, LayerProvidedByContainer);
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
            
            if (Sprite != null)
            {
                FlatRedBall.SpriteManager.RemoveSprite(Sprite);
            }
            if (Hitbox != null)
            {
                FlatRedBall.Math.Geometry.ShapeManager.Remove(Hitbox);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
            CustomDestroy();
        }
        public virtual void PostInitialize ()
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            if (Sprite.Parent == null)
            {
                Sprite.CopyAbsoluteToRelative();
                Sprite.AttachTo(this, false);
            }
            Sprite.Texture = PZiv;
            Sprite.FlipHorizontal = false;
            Sprite.FlipVertical = true;
            Sprite.TextureScale = 0.3f;
            Sprite.Visible = true;
            if (mHitbox.Parent == null)
            {
                mHitbox.CopyAbsoluteToRelative();
                mHitbox.AttachTo(this, false);
            }
            Hitbox.Visible = false;
            FlatRedBall.Math.Geometry.Point[] HitboxPoints = new FlatRedBall.Math.Geometry.Point[] { new FlatRedBall.Math.Geometry.Point(0, 16), new FlatRedBall.Math.Geometry.Point(16, 0), new FlatRedBall.Math.Geometry.Point(-16, 0),  new FlatRedBall.Math.Geometry.Point(0, 16) };
            Hitbox.Points = HitboxPoints;
            mGeneratedCollision = new FlatRedBall.Math.Geometry.ShapeCollection();
            mGeneratedCollision.Polygons.AddOneWay(mHitbox);
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public virtual void AddToManagersBottomUp (FlatRedBall.Graphics.Layer layerToAddTo)
        {
            AssignCustomVariables(false);
        }
        public virtual void RemoveFromManagers ()
        {
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
            if (Sprite != null)
            {
                FlatRedBall.SpriteManager.RemoveSpriteOneWay(Sprite);
            }
            if (Hitbox != null)
            {
                FlatRedBall.Math.Geometry.ShapeManager.RemoveOneWay(Hitbox);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
        }
        public virtual void AssignCustomVariables (bool callOnContainedElements)
        {
            if (callOnContainedElements)
            {
            }
            Sprite.Texture = PZiv;
            Sprite.FlipHorizontal = false;
            Sprite.FlipVertical = true;
            Sprite.TextureScale = 0.3f;
            Sprite.Visible = true;
            Hitbox.Visible = false;
            Drag = 1f;
            MovementSpeed = 1f;
        }
        public virtual void ConvertToManuallyUpdated ()
        {
            this.ForceUpdateDependenciesDeep();
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(this);
            FlatRedBall.SpriteManager.ConvertToManuallyUpdated(Sprite);
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
                        FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("TankStaticUnload", UnloadStaticContent);
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
                        FlatRedBall.FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("TankStaticUnload", UnloadStaticContent);
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
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(Sprite);
            FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(Hitbox);
        }
        public virtual void MoveToLayer (FlatRedBall.Graphics.Layer layerToMoveTo)
        {
            var layerToRemoveFrom = LayerProvidedByContainer;
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(Sprite);
            }
            FlatRedBall.SpriteManager.AddToLayer(Sprite, layerToMoveTo);
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(Hitbox);
            }
            FlatRedBall.Math.Geometry.ShapeManager.AddToLayer(Hitbox, layerToMoveTo);
            LayerProvidedByContainer = layerToMoveTo;
        }
    }
}
