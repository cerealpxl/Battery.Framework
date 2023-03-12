using Battery.Framework;
using Battery.Framework.SDL2;

namespace HelloWorld;

public class Program
{
    /// <summary>
    ///     The Game instance.
    /// </summary>
    public static Game Project = new Game(new SDLPlatform());

    /// <summary>
    ///     Runs the Hello World.
    ///     Draws a white rectangle to the screen.
    /// </summary>
    public static void Main()
    {
        Batch? batch = null;

        Project.OnBegin  += () => batch = new Batch(Project);
        Project.OnRender += () =>
        {
            Project.Graphics.Clear(Project.Platform, Color.Black);
            batch?.Clear();
            batch?.Rectangle(8, 8, 16, 16, Color.White);
            batch?.Present();
        };

        Project.Launch("Hello World", 640, 480);
    }
}