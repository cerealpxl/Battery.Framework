using System.Numerics;
using Battery.Framework;

namespace Battery.Samples;

public class Program
{
    public static void Main(string[] args)
    {
        var game = new Game();
        Mesh?           mesh     = null;
        ShaderMaterial? material = null;
        Bitmap          bitmap   = new Bitmap("test.jpg");
        Texture?        texture  = null;
        
        game.OnBegin += () =>
        {
            mesh     = game.Graphics.CreateMesh();
            material = new ShaderMaterial(game.Graphics.CreateDefaultShader());
            texture  = game.Graphics.CreateTexture(bitmap);
        };
        
        game.OnRender += () =>
        {
            if (mesh == null || material == null || texture == null)
                return;

            var pass = new RenderPass(
                mesh,
                material
            );

            mesh.Clear();
            mesh.AddTriangle(
                new Vertex(new Vector2( 0.5f,  0.5f), Vector2.Zero, Color.White, 255, 0, 0),
                new Vertex(new Vector2(-0.5f,  0.5f), Vector2.UnitX, Color.White, 255, 0, 0),
                new Vertex(new Vector2( 0.0f, -0.5f), new Vector2(0.5f, 1.0f), Color.White, 255, 0, 0)
            );

            material.SetUniform("u_Matrix", Matrix4x4.Identity);
            material.SetUniform("u_Texture", texture);
            game.Graphics.Present(pass);
        };

        game.Launch("Hello window", 640, 480);
    }
}