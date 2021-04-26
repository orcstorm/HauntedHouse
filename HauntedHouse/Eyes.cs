using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace HauntedHouse
{
    class Eyes
    {
        public Texture2D[] Textures;
        public Texture2D MatchTexture;
        public Vector2 Location;
        public float Speed;
        public Texture2D CurrentTexture;
        public Vector2 BackgroundBuffer;
        public bool MatchIsLit;
        public long MatchLitTick;
        public long MatchLitDuration = 20000000;
        public Circle lightCircle;

        public Eyes(Texture2D[] textures, Vector2 location, float speed)
        {
            this.BackgroundBuffer = new Vector2(location.X, location.Y);
            this.Speed = speed;
            this.Textures = textures;
            this.CurrentTexture = Textures[0];
            this.MatchIsLit = false;
            this.MatchTexture = textures[5];
            this.Location = new Vector2(BackgroundBuffer.X / 2f, BackgroundBuffer.Y / 2f);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Location.X, (int)Location.Y, CurrentTexture.Width, CurrentTexture.Height);
        }

        public Circle GetBoundingCircle()
        {
            return new Circle((int)Location.X, (int)Location.Y, (int)MatchTexture.Width / 2);
        }
        
    }
}
