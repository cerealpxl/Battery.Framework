using System.Numerics;
using Battery.Framework;
using Battery.Framework.SDL2;
using Battery.Engine;

namespace Platformer;

public class Program
{
    public static Game Project = new Game(new SDLPlatform());
    
    
    public static void Main(string[] args)
    {
        var manager = Project.Add<SceneManager>();

        Project.OnBegin += () =>
        {
            manager.Current.AddSystem<UpdateSystem>();
            manager.Current.AddSystem<RenderSystem>();
            manager.Current.AddEntity(new Entity(new Player())).X = 16;

            var camera = new Camera(Project.Graphics, 320, 180);
            camera.Scale = new Vector2(2);

            var coll1 = new BoxCollider(Mover.Solid, new Rectangle(0, 0, 16, 180));
            var coll2 = new BoxCollider(Mover.Solid, new Rectangle(0, 164, 640, 16));
            manager.Current.AddEntity(new Entity(coll1, new ColliderRenderer(coll1)));
            manager.Current.AddEntity(new Entity(coll2, new ColliderRenderer(coll2)));
            manager.Current.AddEntity(new Entity(camera));
        };

        Project.Launch("Platformer", 640, 360);
    }
}
