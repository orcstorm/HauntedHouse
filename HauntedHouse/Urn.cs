using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

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
        private SoundEffect found;

        public Urn(Vector2 BackgroundBuffer, Texture2D[] UrnTextures, SoundEffect found)
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
            this.found = found;
            SetUrnLocations();
        }

        private void SetUrnLocations()
        {
            UrnHandleLLocation = RandomPoint(UrnHandleLTexture);
            UrnHandleRLocation = RandomPoint(UrnHandleRTexture);
            UrnCenterLocation = RandomPoint(UrnCenterTexture);
        }

        private Vector2 RandomPoint(Texture2D texture)
        {
            var x = (float)random.NextDouble() * BackgroundBuffer.X;
            var y = (float)random.NextDouble() * BackgroundBuffer.Y;

            if(x <= 30f) {
                x = 30f;
            } else if(x > (BackgroundBuffer.X - (30 - texture.Width)))
            {
                x = BackgroundBuffer.X - 30f - texture.Width;
            }

            if(y <= 30)
            {
                y = 30f;
            } else if (y > BackgroundBuffer.Y - 30 - texture.Height)
            {
                y = BackgroundBuffer.Y - 30 - texture.Height;
            }

            Debug.WriteLine(texture.Name + " " + x + " " + y);
            return new Vector2(x, y);
        }

        public void CheckCollisions(Eyes eyes)
        {
            var eyeBox = eyes.GetBoundingBox();

            if(UrnCenterIsFound == false && IsColliding(GetBoundingBox(UrnCenterLocation, UrnCenterTexture), eyeBox) == true && eyes.MatchIsLit == true) {
                found.Play();
                UrnCenterIsFound = true;
            }

            if(UrnHandleLIsFound == false && IsColliding(GetBoundingBox(UrnHandleLLocation, UrnHandleLTexture), eyeBox) == true && eyes.MatchIsLit == true)
            {
                found.Play();
                UrnHandleLIsFound = true;
            }

            if (UrnHandleRIsFound == false && IsColliding(GetBoundingBox(UrnHandleRLocation, UrnHandleRTexture), eyeBox) == true && eyes.MatchIsLit == true)
            {
                found.Play();
                UrnHandleRIsFound = true;
            }
        }

        private bool IsColliding(Rectangle boxA, Rectangle boxB)
        {
            return boxA.Left < boxB.Right &&
            boxA.Right > boxB.Left &&
            boxA.Top < boxB.Bottom &&
            boxA.Bottom > boxB.Top;
        }

        private Rectangle GetBoundingBox(Vector2 vector, Texture2D texture)
        {
           return new Rectangle((int)vector.X, (int)vector.Y, texture.Width, texture.Height);
        }

        public bool IsCenterLit(Circle circle)
        {
            var rectangle = GetBoundingBox(UrnCenterLocation, UrnCenterTexture);
            return DoRectangleCircleOverlap(circle, rectangle);
        }

        public bool IsLeftLit(Circle circle)
        {
            var rectangle = GetBoundingBox(UrnHandleLLocation, UrnHandleLTexture);
            return DoRectangleCircleOverlap(circle, rectangle);
        }

        public bool IsRightLit(Circle circle)
        {
            var rectangle = GetBoundingBox(UrnHandleRLocation, UrnHandleRTexture);
            return DoRectangleCircleOverlap(circle, rectangle);
        }
        private static bool DoRectangleCircleOverlap(Circle cir, Rectangle rect)
        {

            // Get the rectangle half width and height
            float rW = (rect.Width) / 2;
            float rH = (rect.Height) / 2;

            // Get the positive distance. This exploits the symmetry so that we now are
            // just solving for one corner of the rectangle (memory tell me it fabs for 
            // floats but I could be wrong and its abs)
            float distX = Math.Abs(cir.Center.X - (rect.Left + rW));
            float distY = Math.Abs(cir.Center.Y - (rect.Top + rH));

            if (distX >= cir.Radius + rW || distY >= cir.Radius + rH)
            {
                // Outside see diagram circle E
                return false;
            }
            if (distX < rW || distY < rH)
            {
                // Inside see diagram circles A and B
                return true; // touching
            }

            // Now only circles C and D left to test
            // get the distance to the corner
            distX -= rW;
            distY -= rH;

            // Find distance to corner and compare to circle radius 
            // (squared and the sqrt root is not needed
            if (distX * distX + distY * distY < cir.Radius * cir.Radius)
            {
                // Touching see diagram circle C
                return true;
            }
            return false;
        }

        public int PiecesFound()
        {
            var i = 0;
            if(UrnCenterIsFound == true)
            {
                i = i + 1;
            }
            if (UrnHandleLIsFound == true)
            {
                i = i + 1;
            }
            if (UrnHandleRIsFound == true)
            {
                i = i + 1;
            }
            return i;
        }

    }
}
