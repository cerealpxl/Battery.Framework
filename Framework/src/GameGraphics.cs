namespace Battery.Framework;

/// <summary>
///     Class that provides the graphics backend.
/// </summary>
public abstract class GameGraphics
{
    /// <summary>
    ///     The game to which the platform belongs to.
    /// </summary>
    public Game Game { get; private set; }

    /// <summary>
    ///     Initialize a new instance of the <see cref="GameGraphics" /> class. 
    /// </summary>
    /// <param name="game">The game to which the graphics belongs to.</param>
    public GameGraphics(Game game)
    {
        Game = game;
    }
    /// <summary>
    ///     Creates the default graphics device.
    /// </summary>
    public static GameGraphics CreateDefault(Game instance)
    {
        return new OpenGLGraphics(instance);
    }

    /// <summary>
    ///     Creates a new <see cref="Shader" /> of this graphics implementations.
    /// </summary>
    /// <param name="vertex">The vertex source code.</param>
    /// <param name="fragment">The fragment source code.</param>
    public abstract Shader CreateShader(string vertex, string fragment);

    /// <summary>
    ///     Creates a new <see cref="Shader" /> with the default batcher code.
    /// </summary>
    public abstract Shader CreateDefaultShader();

    /// <summary>
    ///     Creates a new <see cref="Mesh" /> of this graphics implementation.
    /// </summary>
    public abstract Mesh<T> CreateMesh<T>() where T : struct, IVertex;

    /// <summary>
    ///     Creates a new <see cref="Texture" /> of this graphics implementation.
    /// </summary>
    /// <param name="image">The image to use.</param>
    public abstract Texture CreateTexture(Image image);

    /// <summary>
    ///     Creates a new <see cref="Surface" /> of this graphics implementation.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public abstract Surface CreateSurface(int width, int height);

    /// <summary>
    ///     Called to begin the graphics after the platform initialization.
    /// </summary>
    public abstract void Begin();

    /// <summary>
    ///     Called to end the graphics.
    /// </summary>
    public abstract void End();

    /// <summary>
    ///     Presents the given <see cref="RenderPass" />.
    /// </summary>
    /// <param name="pass">The <see cref="RenderPass" /> to use.</param>
    public abstract void Present<T>(RenderPass<T> pass) where T : struct, IVertex;

    /// <summary>
    ///     Clear the specified <see cref="RenderTarget" /> by using the specified color.
    /// </summary>
    /// <param name="target">The target to clear..</param>
    /// <param name="color">The color to use.</param>
    public abstract void Clear(RenderTarget target, Color color);
}