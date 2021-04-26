using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HauntedHouse
{
    class Wall
    {
        public Vector2 location;
        public Texture2D texture;

        public Wall(Vector2 location, Texture2D texture)
        {
            this.location = location;
            this.texture = texture;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)location.X, (int)location.Y, texture.Width,texture.Height);
        }
    }
}
