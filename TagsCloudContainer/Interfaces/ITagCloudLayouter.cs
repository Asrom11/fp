using System.Drawing;

namespace TagsCloudContainer.Interfaces;

public interface ITagCloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
    IEnumerable<Rectangle> GetRectangles();
}