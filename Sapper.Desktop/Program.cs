using LX;
using Sapper.Views;

namespace Sapper
{
    class Program
    {
        static void Main(string[] args)
        {
            App.OnRun += () => new MainForm();
            App.Run();
        }
    }
}
