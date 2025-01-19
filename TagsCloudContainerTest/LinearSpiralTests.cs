using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.PointGenerators;

namespace TagsCloudContainerTest;

public class LinearSpiralTests
{
    private Point center;
    private LinearSpiral generator;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        generator = new LinearSpiral(center);
    }

    [Test]
    public void GetNextPoint_FirstPointShouldBeOnInitialRadius()
    {
        var firstPoint = generator.GetNextPoint();
        var distance = firstPoint.DistanceFromCenter(center);
        
        distance.Should().Be(10.0);
    }

    [Test]
    public void GetNextPoint_ShouldGeneratePointsWithIncreasingRadius()
    {
        var firstPoint = generator.GetNextPoint();
        var firstDistance = firstPoint.DistanceFromCenter(center);
        
        for (var i = 0; i < 13; i++)
        {
            generator.GetNextPoint();
        }

        var nextPoint = generator.GetNextPoint();
        var nextDistance = nextPoint.DistanceFromCenter(center);

        nextDistance.Should().BeGreaterThan(firstDistance);
    }

    [Test]
    public void GetNextPoint_ShouldNotGenerateConsecutiveDuplicatePoints()
    {
        const int numberOfPoints = 100;
        Point? previousPoint = null;

        for (var i = 0; i < numberOfPoints; i++)
        {
            var currentPoint = generator.GetNextPoint();
            if (previousPoint != null)
            {
                currentPoint.Should().NotBe(previousPoint);
            }
            previousPoint = currentPoint;
        }
    }
}