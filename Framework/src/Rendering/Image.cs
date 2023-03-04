using StbImageSharp;
using StbImageWriteSharp;

namespace Battery.Framework;

/// <summary>
///     Class that stores a 2D image.
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
    ///     Array that store the pixel data of the Image.
    /// </summary>
    public Color[] Data { get; private set; }

    /// <summary>
    ///     Creates a image with the given color array.
    /// </summary>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
    /// <param name="data">The array containing the pixels of the image.</param>
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
    ///     Creates an empty image filled with the given color.
    /// </summary>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
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
    ///     Creates a image by using the given byte data.
    ///     Used when creating a Font Char.
    /// </summary>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
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
    ///     Load the image from a file in the given path.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    public static unsafe Image FromFile(string file)
        => FromStream(File.OpenRead(file));

    /// <summary>
    ///     Loads the image from the specified file stream.
    /// </summary>
    /// <param name="stream">The file stream to use.</param>
    public static unsafe Image FromStream(FileStream stream)
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
    ///     Saves the Image to a PNG File.
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