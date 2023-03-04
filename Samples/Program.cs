using System.Numerics;
using Battery.Framework;

namespace Battery.Samples;

public class Program
{
    public static void Main(string[] args)
    {
        var game = new Game();
        var x = 0;
        Mesh<Batch.Vertex>?   mesh     = null;
        ShaderMaterial? material = null;
        Image          image   = new Image("test.jpg");
        Texture?        texture  = null;
        Surface?        surface  = null;
        
        game.OnBegin += () =>
        {
            mesh     = game.Graphics.CreateMesh<Batch.Vertex>();
            material = new ShaderMaterial(game.Graphics.CreateDefaultShader());
            texture  = game.Graphics.CreateTexture(image);
            surface  = game.Graphics.CreateSurface(640, 640);
        };

        game.OnUpdate += (GameTime time) =>
        {
            x += 1;
            if (Input.KeyPressed(KeyConstant.A))
                x -= 16;
        };
        
        game.OnRender += (GameTime time) =>
        {
            if (mesh == null || material == null || texture == null || surface == null)
                return;

            var pass = new RenderPass<Batch.Vertex>(
                mesh,
                material
            );

            mesh.Clear();
            mesh.AddTriangle(
                new Batch.Vertex(new Vector2( 0.5f,  0.5f), Vector2.Zero, Color.White, 255, 0, 0),
                new Batch.Vertex(new Vector2(-0.5f,  0.5f), Vector2.UnitX, Color.White, 255, 0, 0),
                new Batch.Vertex(new Vector2( 0.0f, -0.5f), new Vector2(0.5f, 1.0f), Color.White, 255, 0, 0)
            );

            material.SetUniform("u_Matrix", Matrix4x4.Identity);
            material.SetUniform("u_Texture", texture);
            game.Graphics.Clear(surface, Color.Blue);
            pass.Surface = surface;
            game.Graphics.Present(pass);
            pass.Surface = null;
            pass.ClearColor = Color.Black;
            material.SetUniform("u_Texture", (Texture)surface);

            mesh.Clear();
            mesh.AddTriangle(
                new Batch.Vertex(new Vector2( 0.5f,  0.5f), Vector2.Zero, Color.White, 255, 0, 0),
                new Batch.Vertex(new Vector2(-0.5f,  0.5f), Vector2.UnitX, Color.White, 255, 0, 0),
                new Batch.Vertex(new Vector2( 0.0f, -0.5f), new Vector2(0.5f, 1.0f), Color.White, 255, 0, 0)
            );
            
            game.Graphics.Present(pass);

            material.SetUniform("u_Matrix", Matrix4x4.CreateOrthographic(640, 480, 0, 1));
            pass.ClearColor = null;
            mesh.Clear();
            mesh.AddTriangle(
                new Batch.Vertex(new Vector2(x,      15), Vector2.Zero, Color.White, 0, 0, 255),
                new Batch.Vertex(new Vector2(x + 15, 15), Vector2.UnitX, Color.White, 0, 0, 255),
                new Batch.Vertex(new Vector2(x + 7,  32), new Vector2(0.5f, 1.0f), Color.White, 0, 0, 255)
            );
            
            game.Graphics.Present(pass);
        };

        game.Launch("Hello window", 640, 480);
    }
}