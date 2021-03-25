using System;

namespace CIS598_Senior_Project
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ArmadaGame())
                game.Run();
        }
    }
}
