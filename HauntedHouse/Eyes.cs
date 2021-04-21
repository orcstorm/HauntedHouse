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
            this.Location = new Vector2(location.X / 2f, location.Y / 2f);
            this.BackgroundBuffer = new Vector2(location.X, location.Y);
            this.Speed = speed;
            this.Textures = textures;
            this.CurrentTexture = Textures[0];
            this.MatchIsLit = false;
            this.MatchTexture = textures[5];
        }

        public void UpdateEyes(KeyboardState kstate, GameTime gameTime)
        {
            
            var seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var ticks = gameTime.TotalGameTime.Ticks;
            
            //check if the match should be extinguished
            if (ticks > MatchLitTick + MatchLitDuration)
            {
                MatchIsLit = false;
            }

            //check direction keys
            if (kstate.IsKeyDown(Keys.Up))
            {
                CurrentTexture = Textures[1];
                if (Location.Y - CurrentTexture.Height >  0)
                {
                    Location.Y -= Speed * seconds;
                }
            }
            else if (kstate.IsKeyDown(Keys.Down))
            {
               CurrentTexture = Textures[2];
               if (Location.Y + CurrentTexture.Height < BackgroundBuffer.Y)
               {
                    Location.Y += Speed * seconds;
               }
                
            }
            else if (kstate.IsKeyDown(Keys.Left))
            {
                CurrentTexture = Textures[3];
                if (Location.X - CurrentTexture.Width / 2f > 0)
                {
                    Location.X -= Speed * seconds;
                } else
                {
                    Location.X = 0 + CurrentTexture.Width / 2f;
                }
            }
            else if (kstate.IsKeyDown(Keys.Right))
            {
                CurrentTexture = Textures[4];
                if(Location.X + CurrentTexture.Width / 2f < BackgroundBuffer.X)
                {
                    Location.X += Speed * seconds;
                } else
                {
                    Location.X = BackgroundBuffer.X - CurrentTexture.Width / 2f;
                }
            }
            else
            {
                CurrentTexture = Textures[0];
            }

            if(kstate.IsKeyDown(Keys.Space) && MatchIsLit == false)
            {
                MatchIsLit = true;
                MatchLitTick = ticks;
            }
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
