using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Options;

namespace TagsCloudContainer;

public class CloudImageRenderer : ITagCloudRenderer
{
    private readonly Random random = new();
    private const float MinFontSize = 12f;
    private const float MaxFontSize = 72f;
    private const float FrequencyMultiplier = 2f;

    public Result<None> Render(IEnumerable<Tag> tags, string outputFilePath, RenderingOptions options)
    {
        var checkResult = CheckFitSize(tags, options);
        if (!checkResult.IsSuccess)
        {
            return checkResult;
        }

        return Result.OfAction(() =>
            {
                using var bitmap = new Bitmap(options.ImageSize.Width, options.ImageSize.Height);
                using var graphics = Graphics.FromImage(bitmap);

                graphics.Clear(options.BackgroundColor);
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                foreach (var tag in tags)
                {
                    var fontSize = Math.Max(MinFontSize, Math.Min(MaxFontSize, tag.Frequency * FrequencyMultiplier));
                    using var font = new Font(options.Font, fontSize, FontStyle.Bold);

                    var color = options.WordColors[random.Next(options.WordColors.Length)];
                    using var brush = new SolidBrush(color);

                    graphics.DrawString(
                        tag.Word,
                        font,
                        brush,
                        tag.Rectangle.Location
                    );
                }

                bitmap.Save(outputFilePath, ImageFormat.Png);
            }, $"Ошибка при сохранении результата в файл: {outputFilePath}");
    }

    private static Result<None> CheckFitSize(IEnumerable<Tag> tags, RenderingOptions options)
    {
        if (!tags.Any())
            return Result.Ok();

        var minX = tags.Min(t => t.Rectangle.Left);
        var minY = tags.Min(t => t.Rectangle.Top);
        var maxX = tags.Max(t => t.Rectangle.Right);
        var maxY = tags.Max(t => t.Rectangle.Bottom);

        if (minX < 0 || minY < 0 ||
            maxX > options.ImageSize.Width ||
            maxY > options.ImageSize.Height)
        {
            return Result.Fail<None>("Облако тегов вышло за пределы изображения.");
        }

        return Result.Ok();
    }
}