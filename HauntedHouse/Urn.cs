using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HauntedHouse
{
    class Urn
    {
        public bool UrnHandleLIsFound;
        public bool UrnHandleRIsFound;
        public bool UrnCenterIsFound;
        public Texture2D UrnHandleLTexture;
        public Texture2D UrnHandleRTexture;
        public Texture2D UrnCenterTexture;
        public Vector2 UrnHandleLLocation;
        public Vector2 UrnHandleRLocation;
        public Vector2 UrnCenterLocation;
        public Vector2 BackgroundBuffer;
        public Texture2D[] UrnTextures;
        private Random random; 

        public Urn(Vector2 BackgroundBuffer, Texture2D[] UrnTextures)
        {
            random = new Random();
            this.BackgroundBuffer = BackgroundBuffer;
            this.UrnTextures = UrnTextures;
            UrnHandleLIsFound = false;
            UrnHandleRIsFound = false;
            UrnCenterIsFound = false;
            UrnHandleLTexture = UrnTextures[0];
            UrnCenterTexture = UrnTextures[1];
            UrnHandleRTexture = UrnTextures[2];
            SetUrnLocations();
        }

        public void CheckCollisions(Rectangle eyeBox)
        {
            if(IsColliding(GetBoundingBox(UrnCenterLocation, UrnCenterTexture), eyeBox) == true) {
                UrnCenterIsFound = true;
            }

            if(IsColliding(GetBoundingBox(UrnHandleLLocation, UrnHandleLTexture), eyeBox) == true)
            {
                UrnHandleLIsFound = true;
            }

            if (IsColliding(GetBoundingBox(UrnHandleRLocation, UrnHandleRTexture), eyeBox) == true)
            {
                UrnHandleRIsFound = true;
            }
        }

        private void SetUrnLocations()
        {
            UrnHandleLLocation = RandomPoint();
            UrnHandleRLocation = RandomPoint();
            UrnCenterLocation = RandomPoint();
        }

        private Vector2 RandomPoint()
        {
            return new Vector2((float)random.NextDouble() * BackgroundBuffer.X, (float)random.NextDouble() * BackgroundBuffer.Y);
        }

        private Rectangle GetBoundingBox(Vector2 vector, Texture2D texture)
        {
           return new Rectangle((int)vector.X, (int)vector.Y, texture.Width, texture.Height);
        }

        private bool IsColliding(Rectangle boxA, Rectangle boxB)
        {
            return boxA.Left < boxB.Right &&
            boxA.Right > boxB.Left &&
            boxA.Top < boxB.Bottom &&
            boxA.Bottom > boxB.Top;
        }
    }
}
