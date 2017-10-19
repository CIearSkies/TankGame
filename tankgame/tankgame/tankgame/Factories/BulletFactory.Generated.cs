using tankgame.Entities;
using System;
using FlatRedBall.Math;
using FlatRedBall.Graphics;
using tankgame.Performance;

namespace tankgame.Factories
{
    public class BulletFactory : IEntityFactory
    {
        public static FlatRedBall.Math.Axis? SortAxis { get; set;}
        public static Bullet CreateNew (float x = 0, float y = 0)
        {
            return CreateNew(null, x, y);
        }
        public static Bullet CreateNew (Layer layer, float x = 0, float y = 0)
        {
            if (string.IsNullOrEmpty(mContentManagerName))
            {
                throw new System.Exception("You must first initialize the factory to use it. You can either add PositionedObjectList of type Bullet (the most common solution) or call Initialize in custom code");
            }
            Bullet instance = null;
            instance = new Bullet(mContentManagerName, false);
            instance.AddToManagers(layer);
            instance.X = x;
            instance.Y = y;
            if (mScreenListReference != null)
            {
                if (SortAxis == FlatRedBall.Math.Axis.X)
                {
                    var index = mScreenListReference.GetFirstAfter(x, Axis.X, 0, mScreenListReference.Count);
                    mScreenListReference.Insert(index, instance);
                }
                else if (SortAxis == FlatRedBall.Math.Axis.Y)
                {
                    var index = mScreenListReference.GetFirstAfter(y, Axis.Y, 0, mScreenListReference.Count);
                    mScreenListReference.Insert(index, instance);
                }
                else
                {
                    // Sort Z not supported
                    mScreenListReference.Add(instance);
                }
            }
            if (EntitySpawned != null)
            {
                EntitySpawned(instance);
            }
            return instance;
        }
        
        public static void Initialize (FlatRedBall.Math.PositionedObjectList<Bullet> listFromScreen, string contentManager)
        {
            mContentManagerName = contentManager;
            mScreenListReference = listFromScreen;
        }
        
        public static void Destroy ()
        {
            mContentManagerName = null;
            mScreenListReference = null;
            SortAxis = null;
            mPool.Clear();
            EntitySpawned = null;
        }
        
        private static void FactoryInitialize ()
        {
            const int numberToPreAllocate = 20;
            for (int i = 0; i < numberToPreAllocate; i++)
            {
                Bullet instance = new Bullet(mContentManagerName, false);
                mPool.AddToPool(instance);
            }
        }
        
        /// <summary>
        /// Makes the argument objectToMakeUnused marked as unused.  This method is generated to be used
        /// by generated code.  Use Destroy instead when writing custom code so that your code will behave
        /// the same whether your Entity is pooled or not.
        /// </summary>
        public static void MakeUnused (Bullet objectToMakeUnused)
        {
            MakeUnused(objectToMakeUnused, true);
        }
        
        /// <summary>
        /// Makes the argument objectToMakeUnused marked as unused.  This method is generated to be used
        /// by generated code.  Use Destroy instead when writing custom code so that your code will behave
        /// the same whether your Entity is pooled or not.
        /// </summary>
        public static void MakeUnused (Bullet objectToMakeUnused, bool callDestroy)
        {
            if (callDestroy)
            {
                objectToMakeUnused.Destroy();
            }
        }
        
        
            static string mContentManagerName;
            static PositionedObjectList<Bullet> mScreenListReference;
            static PoolList<Bullet> mPool = new PoolList<Bullet>();
            public static Action<Bullet> EntitySpawned;
            object IEntityFactory.CreateNew ()
            {
                return BulletFactory.CreateNew();
            }
            object IEntityFactory.CreateNew (Layer layer)
            {
                return BulletFactory.CreateNew(layer);
            }
            public static FlatRedBall.Math.PositionedObjectList<Bullet> ScreenListReference
            {
                get
                {
                    return mScreenListReference;
                }
                set
                {
                    mScreenListReference = value;
                }
            }
            static BulletFactory mSelf;
            public static BulletFactory Self
            {
                get
                {
                    if (mSelf == null)
                    {
                        mSelf = new BulletFactory();
                    }
                    return mSelf;
                }
            }
    }
}
