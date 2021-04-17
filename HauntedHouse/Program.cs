using System;

namespace HauntedHouse
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new HauntedHouseGame())
                game.Run();
        }
    }
}
