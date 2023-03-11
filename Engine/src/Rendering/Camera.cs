using System.Numerics;
using Battery.Framework;

namespace Battery.Engine;

/// <summary>
///     Provides the position, angle and scale of a single Camera View.
/// </summary>
public class Camera : Component, ITagged
{
    /// <summary>
    ///     Whether the camera can trigger the Render events.
    /// </summary>
    public bool Enabled = true;

    /// <summary>
    ///     Whether the camera will automatically draw the Surface.
    /// </summary>
    public bool Visible = true;

    /// <summary>
    ///     The tag used by the camera.
    /// </summary>
    public Tag Tag { get; set; } = Tag.All;

    /// <summary>
    ///     The Camera's surface.
    /// </summary>
    public Surface Surface;

    /// <summary>
    ///     The position of the Camera.
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The horizontal position of the Camera.
    /// </summary>
    public float X
    {
        get => _position.X;
        set
        {
            if (_position.X != value)
            {
                _position.X = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The vertical position of the Camera.
    /// </summary>
    public float Y
    {
        get => _position.Y;
        set
        {
            if (_position.Y != value)
            {
                _position.Y = value;
                Refresh();
            }
        }
    }
    
    /// <summary>
    ///     The position of the Camera.
    /// </summary>
    public Vector2 Scale
    {
        get => _scale;
        set
        {
            if (_scale != value)
            {
                _scale = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The horizontal position of the Camera.
    /// </summary>
    public float ScaleX
    {
        get => _scale.X;
        set
        {
            if (_scale.X != value)
            {
                _scale.X = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The vertical position of the Camera.
    /// </summary>
    public float ScaleY
    {
        get => _scale.Y;
        set
        {
            if (_scale.Y != value)
            {
                _scale.Y = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The angle of the Camera's rotation.
    /// </summary>
    public float Angle
    {
        get => _angle;
        set
        {
            if (_angle != value)
            {
                _angle = value;
                Refresh();
            }
        }
    }

    public Vector2 Dimensions
    {
        get => _dimensions;
        set
        {
            if (_dimensions != value)
            {
                _dimensions = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The width of the Camera.
    /// </summary>
    public float Width
    {
        get => _dimensions.X;
        set
        {
            if (_dimensions.X != value)
            {
                _dimensions.X = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The height of the Camera.
    /// </summary>
    public float Height
    {
        get => _dimensions.Y;
        set
        {
            if (_dimensions.Y != value)
            {
                _dimensions.Y = value;
                Refresh();
            }
        }
    }

    /// <summary>
    ///     The View Matrix of the Camera.
    /// </summary>
    public Matrix3x2 Matrix;

    /// <summary>
    ///     The <see cref="Color" /> used to clear the camera surface.
    /// </summary>
    public Color ClearColor = Color.Black;

    // The position of the Camera.
    private Vector2 _position;

    // The scale of the Camera.
    private Vector2 _scale = Vector2.One;

    // The angle of the Camera's rotation.
    private float _angle;

    // The dimensions of the Camera.
    private Vector2 _dimensions;

    // The graphics backend used to create this camera.
    private GameGraphics _graphics;

    /// <summary>
    ///     Creates a new instance of the <see cref="Camera" /> component.
    /// </summary>
    /// <param name="graphics">The current Graphics Backend.</param>
    /// <param name="width">The width of the camera.</param>
    /// <param name="height">The height of the camera.</param>
    public Camera(GameGraphics graphics, int width, int height)
    {
        _graphics     = graphics;
        _dimensions.X = width;
        _dimensions.Y = height;
        Surface       = _graphics.CreateSurface((int)Width, (int)Height);
        Refresh(true);
    }

    /// <summary>
    ///     Refresh the view matrix.
    /// </summary>
    public void Refresh(bool matrixOnly = false)
    { 
        Matrix = Matrix3x2.CreateTranslation(-Position) *
            Matrix3x2.CreateRotation(Angle) *
            Matrix3x2.CreateScale(Scale);

        if (!matrixOnly)
        {
            Surface.Dispose();
            Surface = _graphics.CreateSurface((int)Width, (int)Height);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="camera"></param>
    public static explicit operator RenderTarget(Camera camera)
        => camera.Surface;
}