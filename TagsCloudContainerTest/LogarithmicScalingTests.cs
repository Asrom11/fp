using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Options;
using Size = System.Drawing.Size;

namespace TagsCloudContainerTest;

public class LogarithmicScalingTests
{
    private LogarithmicScaling calculator;
    private RenderingOptions options;

    [SetUp]
    public void Setup()
    {
        calculator = new LogarithmicScaling();
        options = new RenderingOptions
        {
            Font = "Arial",
            ImageSize = new Size(1000, 1000)
        };
    }

    [Test]
    public void GetWordSize_ReturnsSizeGreaterThanZero()
    {
        var size = calculator.GetWordSize("test", 1, options);

        size.Width.Should().BeGreaterThan(0);
        size.Height.Should().BeGreaterThan(0);
    }

    [Test]
    public void GetWordSize_HigherFrequencyGivesLargerSize()
    {
        var smallSize = calculator.GetWordSize("test", 1, options);
        var largeSize = calculator.GetWordSize("test", 100, options);

        largeSize.Width.Should().BeGreaterThan(smallSize.Width);
        largeSize.Height.Should().BeGreaterThan(smallSize.Height);
    }

    [Test]
    public void GetWordSize_LongerWordGivesWiderSize()
    {
        var shortWordSize = calculator.GetWordSize("test", 10, options);
        var longWordSize = calculator.GetWordSize("testtesttest", 10, options);

        longWordSize.Width.Should().BeGreaterThan(shortWordSize.Width);
    }

    [Test]
    public void GetWordSize_SameInputGivesSameOutput()
    {
        var size1 = calculator.GetWordSize("test", 10, options);
        var size2 = calculator.GetWordSize("test", 10, options);

        size2.Should().Be(size1);
    }
}