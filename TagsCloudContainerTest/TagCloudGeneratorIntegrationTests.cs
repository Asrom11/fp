using System.Drawing;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Options;

namespace TagsCloudContainerTest;

public class TagCloudGeneratorIntegrationTests
{
    private IContainer container;
    private string tempDirectory;
    private string inputPath;
    private string outputPath;

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        container = DependencyInjectionConfig.BuildContainer();
        tempDirectory = Path.Combine(Path.GetTempPath(), "TagCloudTests");
        Directory.CreateDirectory(tempDirectory);
        inputPath = Path.Combine(tempDirectory, "input.txt");
        outputPath = Path.Combine(tempDirectory, "output.png");
    }

    [OneTimeTearDown]
    public void GlobalCleanup()
    {
        if (Directory.Exists(tempDirectory))
            Directory.Delete(tempDirectory, true);
    }

    [Test]
    public void GenerateCloud_WithSimpleInput_CreatesValidOutput()
    {
        const string testText = @"
            This is a test text
            with multiple words
            some words appear
            multiple times in the text
            test test test
        ";
        File.WriteAllText(inputPath, testText);

        using var scope = container.BeginLifetimeScope();
        var generator = scope.Resolve<ITagCloudGenerator>();
        var options = new RenderingOptions
        {
            BackgroundColor = Color.White,
            WordColors = [Color.Black],
            Font = "Arial",
            ImageSize = new Size(800, 600)
        };

        generator.GenerateCloud(inputPath, outputPath, options);

        File.Exists(outputPath).Should().BeTrue();
        using var image = Image.FromFile(outputPath);
        image.Width.Should().Be(options.ImageSize.Width);
        image.Height.Should().Be(options.ImageSize.Height);
    }

    [Test]
    public void GenerateCloud_WithLargeImageSize_ReportsOutOfRangeErrorAndDoesNotCreateOutputFile()
    {
        const string outRangeError = "Облако тегов вышло за пределы изображения.";
        var random = new Random(42);
        var words = new[] { "test", "cloud", "generator", "word", "frequency" };
        var testText = string.Join(" ",
            Enumerable.Range(0, 1000)
                .Select(_ => words[random.Next(words.Length)]));

        File.WriteAllText(inputPath, testText);

        using var scope = container.BeginLifetimeScope();
        var generator = scope.Resolve<ITagCloudGenerator>();
        var options = new RenderingOptions
        {
            BackgroundColor = Color.White,
            WordColors = [Color.Black],
            Font = "Arial",
            ImageSize = new Size(10000, 10000)
        };

        var result = generator.GenerateCloud(inputPath, outputPath, options);

        result.Error.Should().Be(outRangeError);
        File.Exists(outputPath).Should().BeFalse();
    }
}