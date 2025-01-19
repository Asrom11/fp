using System.Drawing;

namespace TagsCloudContainer.Options;

public class RenderingOptions
{
    public Color BackgroundColor { get; set; } = Color.White;

    public Color[] WordColors { get; set; } =
    {
        Color.Black,
        Color.FromArgb(255, 65, 105, 225),
        Color.FromArgb(255, 34, 139, 34),
        Color.FromArgb(255, 178, 34, 34),
        Color.FromArgb(255, 148, 0, 211)
    };

    public string Font { get; set; } = "Times New Roman";
    public Size ImageSize { get; set; } = new(1000, 1000);
}