using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace HauntedHouse
{
    
    class Room
    {
        public List<Wall> walls;
        public Vector2 backgroundBuffer;
        public int id;
        
        public Room(Vector2 backgroundBuffer, int id)
        {
            this.backgroundBuffer = backgroundBuffer;
            this.walls = new List<Wall> { };
            this.id = id;
        }

        public void AddWall(Vector2 location, Texture2D texture)
        {
            walls.Add(new Wall(location, texture));
        }

        public bool checkCollisions(Rectangle eyeBox)
        {
            foreach(Wall wall in walls)
            {
                if(AABB(wall.GetRectangle(), eyeBox) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AABB(Rectangle boxA, Rectangle boxB)
        {
            return boxA.Left < boxB.Right &&
            boxA.Right > boxB.Left &&
            boxA.Top < boxB.Bottom &&
            boxA.Bottom > boxB.Top;
        }

    }
}
