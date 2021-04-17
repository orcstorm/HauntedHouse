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

        public Eyes(Texture2D[] textures, Vector2 location, float speed)
        {
            this.Location = new Vector2(location.X / 2, location.Y / 2);
            this.Speed = speed;
            this.Textures = textures;
            this.CurrentTexture = Textures[0];
        }

        public void UpdateEyes(KeyboardState kstate, float seconds)
        {
            if (kstate.IsKeyDown(Keys.Up))
            {
                CurrentTexture = Textures[1];
                Location.Y -= Speed * seconds;
            }
            else if (kstate.IsKeyDown(Keys.Down))
            {
               CurrentTexture = Textures[2];
               Location.Y += Speed * seconds;
            }
            else if (kstate.IsKeyDown(Keys.Left))
            {
                CurrentTexture = Textures[3];
                Location.X -= Speed * seconds;
            }
            else if (kstate.IsKeyDown(Keys.Right))
            {
                CurrentTexture = Textures[4];
                Location.X += Speed * seconds;
            }
            else
            {
                CurrentTexture = Textures[0];
            }
        }
    }


}
