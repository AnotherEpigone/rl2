using Autofac;

namespace Roguelike2
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var container = AutofacSetup.CreateContainer();

            var game = container.Resolve<Rl2>();
            game.Run();
        }
    }
}
