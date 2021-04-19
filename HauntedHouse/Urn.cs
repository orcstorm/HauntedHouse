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
    }
}
