using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HauntedHouse
{
    class Eyes
    {
        public Texture2D[] Textures;
        public Vector2 Location;
        public float Speed;
        public Texture2D CurrentTexture;
        public Vector2 BackgroundBuffer;

        public Eyes(Texture2D[] textures, Vector2 location, float speed)
        {
            this.Location = new Vector2(location.X / 2, location.Y / 2);
            this.BackgroundBuffer = new Vector2(location.X, location.Y);
            this.Speed = speed;
            this.Textures = textures;
            this.CurrentTexture = Textures[0];
        }

        public void UpdateEyes(KeyboardState kstate, float seconds)
        {
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
                    Location.X = 0 + CurrentTexture.Width / 2;
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
        }
    }


}
