using System.Drawing;

namespace TagsCloudContainer.Extensions;

public static class PointExtensions
{
    public static double DistanceFromCenter(this Point point, Point center)
    {
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}