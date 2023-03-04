using System.Numerics;
using Battery.Framework;
using Battery.Framework.SDL2;

namespace Battery.Samples;

public class Program
{
    public static void Main(string[] args)
    {
        var game = new Game(new SDLPlatform());
        
        Batch?              batch    = null;
        Mesh<Batch.Vertex>? mesh     = null;
        ShaderMaterial?     material = null;
        Image           image      = Image.FromFile("test.jpg");
        Texture?        texture    = null;
        
        game.OnBegin  += () => 
        {
            batch    = new Batch(game.Graphics);
            mesh     = game.Graphics.CreateMesh<Batch.Vertex>();
            texture  = game.Graphics.CreateTexture(image);
            material = new ShaderMaterial(game.Graphics.CreateDefaultShader());
        };

        game.OnRender += (GameTime time) =>
        {
            if (texture == null)
                return;

            batch?.Clear();
            batch?.Texture(texture, new Vector2(32, 16));
            batch?.Rectangle(16, 16, 16, 16, Color.White);
            batch?.Rectangle(24, 24, 16, 16, Color.Red);
            batch?.Present(Matrix4x4.CreateOrthographicOffCenter(0, 640, 480, 0, 0, 1));
            
            if (!(mesh == null || material == null))
            {
                var pass = new RenderPass<Batch.Vertex>(
                    mesh,
                    material
                );
                
                pass.ClearColor = null;
                mesh.Clear();
                mesh.AddTriangle(
                    new Batch.Vertex(new Vector2( 0.5f,  0.5f), Vector2.Zero, Color.Red, 255, 0, 255),
                    new Batch.Vertex(new Vector2(-0.5f,  0.5f), Vector2.UnitX, Color.Blue, 255, 0, 255),
                    new Batch.Vertex(new Vector2( 0.0f, -0.5f), new Vector2(0.5f, 1.0f), Color.Green, 255, 0, 255)
                );

                material.SetUniform("u_Matrix", Matrix4x4.Identity);
                game.Graphics.Present(pass);
            }
        };

        game.Launch("Batch test.", 640, 480);
    }
    public static void fMain(string[] args)
    {
        var game = new Game(new SDLPlatform());
        var x = 0;
        Mesh<Batch.Vertex>?   mesh = null;
        ShaderMaterial? material   = null;
        Image           image      = Image.FromFile("test.jpg");
        Texture?        texture    = null;
        Surface?        surface    = null;
        
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