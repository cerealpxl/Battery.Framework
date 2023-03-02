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
    ///     Creates a new shader of this graphics implementations.
    /// </summary>
    /// <param name="vertex">The vertex source code.</param>
    /// <param name="fragment">The fragment source code.</param>
    public abstract Shader CreateShader(string vertex, string fragment);

    /// <summary>
    ///     Creates a new shader with the default batcher code.
    /// </summary>
    public abstract Shader CreateDefaultShader();

    /// <summary>
    ///     Creates a new mesh of this graphics implementation.
    /// </summary>
    public abstract Mesh CreateMesh();

    /// <summary>
    ///     Creates a new texture of this graphics implementation.
    /// </summary>
    /// <param name="bitmap">The bitmap to use.</param>
    public abstract Texture CreateTexture(Bitmap bitmap);

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
    public abstract void Present(RenderPass pass);
}