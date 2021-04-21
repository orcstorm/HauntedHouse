using Microsoft.Xna.Framework;

namespace HauntedHouse
{
    public struct Circle
    {

        public Point Center;

        public int Radius;

        public Circle(int x, int y, int radius)
        {
            Center = new Point(x, y);
            Radius = radius;
        }
    }
}
