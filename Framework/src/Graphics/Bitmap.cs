using StbImageSharp;
using StbImageWriteSharp;

namespace Battery.Framework;

/// <summary>
///     Class that stores a 2D image.
/// </summary>
public class Bitmap
{
    /// <summary>
    ///     The Width of the Bitmap, in Pixels.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    ///     The Height of the Bitmap, in Pixels.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    ///     Array that store the pixel data of the Bitmap.
    /// </summary>
    public Color[] Data { get; private set; }

    /// <summary>
    ///     Path to the image file.
    /// </summary>
    public string Path { get; private set; } = "";

    /// <summary>
    ///     Creates a bitmap with the given color array.
    /// </summary>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    /// <param name="data">The array containing the pixels of the bitmap.</param>
    public Bitmap(int width, int height, Color[] data)
    {
        if (width <= 0 || height <= 0)
            throw new Exception("The width and height of the bitmap must be larger than 0.");

        if (data.Length < width * height)
            throw new Exception("The pixel array doesn't fits the given bitmap dimensions.");

        // Assign variables.
        Width  = width;
        Height = height;
        Data   = data;
    }

    /// <summary>
    ///     Creates an empty bitmap filled with the given color.
    /// </summary>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    /// <param name="color">The color used to fill the bitmap.</param>
    public Bitmap(int width, int height, Color color)
    {
        if (width <= 0 || height <= 0)
            throw new Exception("The width and height of the bitmap must be larger than 0.");
            
        Width  = width;
        Height = height;
        Data   = new Color[width * height];

        Array.Fill(Data, color);
    }

    /// <summary>
    ///     Creates a bitmap by using the given byte data.
    ///     Used when creating a Font char.
    /// </summary>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    /// <param name="data">The bytes of the char.</param>
    public Bitmap(int width, int height, byte[] data)
    {
        Width  = width;
        Height = height;
        Data   = new Color[Width * Height];

        for (int i = 0, j = 0; i < Data.Length; ++ i, ++ j)
            Data[i] = new Color(data[j], data[j], data[j], data[j]);
    }

    /// <summary>
    ///     Load the bitmap from a image in the given path.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    public unsafe Bitmap(string file)
    {
        var stream = File.OpenRead(file);
        var image  = ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);

        // Assign variables.
        Width  = image.Width;
        Height = image.Height;
        Data   = new Color[Width * Height];
        Path   = file;

        for (int i = 0, j = 0; i < Data.Length; ++ i)
        {
            Data[i].R = image.Data[j ++];
            Data[i].G = image.Data[j ++];
            Data[i].B = image.Data[j ++];
            Data[i].A = image.Data[j ++];
        }
    }

    /// <summary>
    ///     Saves the Bitmap to a PNG File.
    /// </summary>
    /// <param name="file">Location of the file.</param>
    public void Save(string file)
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

        File.WriteAllBytes(file, stream.ToArray());
    }
}