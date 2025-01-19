using System.Drawing;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.PointGenerators;

public class LinearSpiral : IPointGenerator
{
    private readonly Point center;
    private double angle;
    private double radius;
    private readonly double radiusStep;
    private Point? lastGeneratedPoint;

    public LinearSpiral(Point center, double radiusStep = 10)
    {
        this.center = center;
        this.radiusStep = radiusStep;
        angle = 0;
        radius = radiusStep;
    }

    public Point GetNextPoint()
    {
        Point newPoint;
        do
        {
            var x = (int)(center.X + radius * Math.Cos(angle));
            var y = (int)(center.Y + radius * Math.Sin(angle));
            
            angle += Math.PI / 6;
            if (angle >= 2 * Math.PI)
            {
                angle = 0;
                radius += radiusStep;
            }

            newPoint = new Point(x, y);
        } while (newPoint == lastGeneratedPoint);

        lastGeneratedPoint = newPoint;
        return newPoint;
    }
}