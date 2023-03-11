using System.Numerics;
using Battery.Framework;
using Battery.Framework.SDL2;
using Battery.Engine;

namespace HelloWorld;

public class Program
{
    public static Game Project = new Game(new SDLPlatform());
    
    public static void Main(string[] args)
    {
        Batch? batch = null;

        var manager = Project.Add<SceneManager>();

        Project.OnBegin += () =>
        {
            batch = new Batch(Project);
            manager.Current.AddSystem<RenderSystem>();
        };
        Project.Launch("Hello World", 640, 480);
    }
}
