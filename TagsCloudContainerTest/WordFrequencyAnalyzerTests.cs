using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

public class WordFrequencyAnalyzerTests
{
    private WordFrequencyAnalyzer analyzer;

    [SetUp]
    public void Setup()
    {
        analyzer = new WordFrequencyAnalyzer();
    }

    [Test]
    public void Analyze_WithEmptyInput_ReturnsEmptyList()
    {
        var result = analyzer.Analyze(Array.Empty<string>());
        
        result.Should().BeEmpty();
    }

    [Test]
    public void Analyze_CountsWordFrequencies()
    {
        var words = new[] { "hello", "world", "hello", "test" };
        
        var result = analyzer.Analyze(words);
        
        result.Should().HaveCount(3);
        result.First().Word.Should().Be("hello");
        result.First().Frequency.Should().Be(2);
    }

    [Test]
    public void Analyze_OrdersByFrequencyDescending()
    {
        var words = new[] { "rare", "common", "common", "common", "medium", "medium" };
        
        var result = analyzer.Analyze(words);
        
        result.Select(t => t.Frequency).Should().BeInDescendingOrder();
        result[0].Word.Should().Be("common");
        result[1].Word.Should().Be("medium");
        result[2].Word.Should().Be("rare");
    }
}