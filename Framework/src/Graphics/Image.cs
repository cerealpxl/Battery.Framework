using StbImageSharp;
using StbImageWriteSharp;

namespace Battery.Framework;

/// <summary>
///     Represents a 2D Image.
///     Provides methods to create and manipulate an image.
/// </summary>
public class Image
{
    /// <summary>
    ///     The Width of the Image, in Pixels.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    ///     The Height of the Image, in Pixels.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    ///     An array that store the pixel data of the Image.
    /// </summary>
    public Color[] Data { get; private set; }

    /// <summary>
    ///     Creates a new instance of <see cref="Image"/> with the given pixels array.
    /// </summary>
    /// <param name="width">The Width of the Image.</param>
    /// <param name="height">The Height of the Image.</param>
    /// <param name="data">The array containing the pixels of the Image.</param>
    public Image(int width, int height, Color[] data)
    {
        if (width <= 0 || height <= 0)
            throw new Exception("The width and height of the image must be larger than 0.");

        if (data.Length < width * height)
            throw new Exception("The pixel array doesn't fits the given image dimensions.");

        // Assign variables.
        Width  = width;
        Height = height;
        Data   = data;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="Image"/> filled with the given color.
    /// </summary>
    /// <param name="width">The Width of the Image.</param>
    /// <param name="height">The Height of the Image.</param>
    /// <param name="color">The color used to fill the image.</param>
    public Image(int width, int height, Color color)
    {
        if (width <= 0 || height <= 0)
            throw new Exception("The width and height of the image must be larger than 0.");
            
        Width  = width;
        Height = height;
        Data   = new Color[width * height];

        Array.Fill(Data, color);
    }

    /// <summary>
    ///     Creates a new instance of <see cref="Image"/> by using the given byte data.
    ///     A single byte represents the value of all the color components.
    /// </summary>
    /// <param name="width">The Width of the image.</param>
    /// <param name="height">The Height of the image.</param>
    /// <param name="data">The bytes of the char.</param>
    public Image(int width, int height, byte[] data)
    {
        Width  = width;
        Height = height;
        Data   = new Color[Width * Height];

        for (int i = 0, j = 0; i < Data.Length; ++ i, ++ j)
            Data[i] = new Color(data[j], data[j], data[j], data[j]);
    }

    /// <summary>
    ///     Load the image from a PNG file in the specified path.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    public static Image FromFile(string path)
    {
        return FromStream(File.OpenRead(path));
    }

    /// <summary>
    ///     Loads the image from the given File Stream.
    /// </summary>
    /// <param name="stream">The File Stream to use.</param>
    public static Image FromStream(FileStream stream)
    {
        // Assign variables.
        var rawImage = ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);
        var data = new Color[rawImage.Width * rawImage.Height];

        for (int i = 0, j = 0; i < data.Length; ++ i)
        {
            data[i].R = rawImage.Data[j ++];
            data[i].G = rawImage.Data[j ++];
            data[i].B = rawImage.Data[j ++];
            data[i].A = rawImage.Data[j ++];
        }

        return new Image(rawImage.Width, rawImage.Height, data);
    }

    /// <summary>
    ///     Saves the Image to a PNG file in the specified path.
    /// </summary>
    /// <param name="path">The location of the file.</param>
    public void Save(string path)
    {
        var stream = new MemoryStream();
        var writer = new ImageWriter();

        unsafe
        {
            // Premultiplies the Color Data to use in PNG.
            fixed (void* ptr = Data)
            {
                byte* rgba = (byte*)ptr;

                for (int i = 0, len = Data.Length * 4; i < len ; i += 4)
                {
                    rgba[i + 0] = (byte)(rgba[i + 0] * rgba[i + 3] / 255);
                    rgba[i + 1] = (byte)(rgba[i + 1] * rgba[i + 3] / 255);
                    rgba[i + 2] = (byte)(rgba[i + 2] * rgba[i + 3] / 255);
                }

                writer.WritePng(rgba, Width, Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);
            }
        }

        File.WriteAllBytes(path, stream.ToArray());
    }

    /// <summary>
    ///     Sets the image pixels at the given destination.
    /// </summary>
    /// <param name="data">The data to set.</param>
    /// <param name="destination">The destination of the pixel data.</param>
    public void SetPixels(Memory<Color> data, RectangleI destination)
    {
        var src = data.Span;
        var dst = new Span<Color>(Data);

        for (int y = 0; y < destination.Height; y ++)
        {
            var from = src.Slice(y * destination.Width, destination.Width);
            var to   = dst.Slice(destination.X + (destination.Y + y) * Width, destination.Width);

            from.CopyTo(to);
        }
    }

    /// <summary>
    ///     Gets a pixel area in the bitmap.
    /// </summary>
    /// <param name="destination">The memory to assign with the pixels.</param>
    /// <param name="destinationRect"></param>
    /// <param name="sourceRect"></param>
    public void GetPixels(Memory<Color> destination, RectangleI destinationRect, RectangleI sourceRect)
    {
        var src = new Span<Color>(Data);
        var dst = destination.Span;

        // Can't be outside of the source image.
        if (sourceRect.Left   < 0)      sourceRect.X      = 0;
        if (sourceRect.Top    < 0)      sourceRect.Y      = 0;
        if (sourceRect.Right  > Width)  sourceRect.Width  = Width - sourceRect.X;
        if (sourceRect.Bottom > Height) sourceRect.Height = Height - sourceRect.Y;

        // Can't be larger than our destination.
        if (sourceRect.Width > destinationRect.Width - destinationRect.X)
            sourceRect.Width = destinationRect.Width - destinationRect.X;

        if (sourceRect.Height > destinationRect.Height - destinationRect.Y)
            sourceRect.Height = destinationRect.Height - destinationRect.Y;

        // Fiinally, gets the pixel data.
        for (int y = 0; y < sourceRect.Height; y++)
        {
            var from = src.Slice(sourceRect.X      + (sourceRect.Y      + y) * Width,                 sourceRect.Width);
            var to   = dst.Slice(destinationRect.X + (destinationRect.Y + y) * destinationRect.Width, sourceRect.Width);

            from.CopyTo(to);
        }
    }
}