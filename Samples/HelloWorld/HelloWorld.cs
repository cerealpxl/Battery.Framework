using System.Numerics;
using Battery.Framework;
using Battery.Framework.SDL2;

namespace HelloWorld;

public class Program
{
    public static Game Project = new Game(new SDLPlatform());
    public static void Main(string[] args)
    {
        Batch? batch = null;

        Project.OnBegin  += (             ) => batch = new Batch(Project);
        Project.OnRender += (GameTime time) =>
        {
            Project.Graphics.Clear(Project.Platform, Color.Black);
            batch?.Clear();

            batch?.PushMatrix(Matrix3x2.CreateRotation((float)time.Elapsed.TotalSeconds));
            batch?.Rectangle(-8, -8, 16, 16, Color.White);
            batch?.PopMatrix();
            
            batch?.Present();
        };

        Project.Launch("Hello World", 640, 480);
    }
}
