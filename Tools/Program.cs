using System.Numerics;
using Battery.Framework;
using Battery.Framework.SDL2;
using Battery.Engine;

namespace Battery.Tools;

public class Program
{
    public static Game Project = new Game(new SDLPlatform());
    
    
    public static void Main(string[] args)
    {
        var manager = Project.Add(new GuiManager(Project));

        Project.Launch("Platformer", 960, 540);
    }
}
