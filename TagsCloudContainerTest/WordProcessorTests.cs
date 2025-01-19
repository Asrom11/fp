using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTest;

[TestFixture]
public class WordProcessorTests
{
    private WordProcessor processor;
    private string boringWordsFilePath;

    [SetUp]
    public void Setup()
    {
        boringWordsFilePath = Path.GetTempFileName();
        File.WriteAllLines(boringWordsFilePath, new[] { "the", "a", "an" });

        processor = new WordProcessor(boringWordsFilePath);
    }

    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(boringWordsFilePath))
        {
            File.Delete(boringWordsFilePath);
        }
    }

    [Test]
    public void ProcessWords_WithNullInput_ReturnsFailResult()
    {
        var result = processor.ProcessWords(null);

        result.IsSuccess.Should().BeFalse("при null надо возвращать ошибку");
        result.Error.Should().Contain("Value cannot be null", "должна присутствовать ошибка о null");
    }

    [Test]
    public void ProcessWords_WithEmptyInput_ReturnsEmptyCollection()
    {
        var result = processor.ProcessWords(Array.Empty<string>());

        result.IsSuccess.Should().BeTrue("не должно быть ошибок для пустого списка");
        result.Error.Should().BeNullOrEmpty("при успешной обработке ошибки отсутствуют");
        result.Value.Should().BeEmpty("результат должен быть пустым для пустого ввода");
    }

    [Test]
    public void ProcessWords_RemovesBoringWords()
    {
        var words = new[] { "Hello", "the", "World", "a" };
        var expected = new List<string> { "hello", "world" };

        var result = processor.ProcessWords(words);

        result.IsSuccess.Should().BeTrue("должен успешно обработать массив");
        result.Error.Should().BeNullOrEmpty("при успешной обработке ошибки отсутствуют");
        result.Value.Should().BeEquivalentTo(expected, "должны оставаться только осмысленные слова");
    }

    [Test]
    public void ProcessWords_ConvertsToLowerCase()
    {
        var words = new[] { "Hello", "WORLD" };
        var expected = new List<string> { "hello", "world" };

        var result = processor.ProcessWords(words);

        result.IsSuccess.Should().BeTrue("должен успешно обработать массив");
        result.Error.Should().BeNullOrEmpty("при успешной обработке ошибки отсутствуют");
        result.Value.Should().BeEquivalentTo(expected, "должен конвертировать слова в нижний регистр");
    }

    [Test]
    public void ProcessWords_RemovesWhitespace()
    {
        var words = new[] { "  hello  ", "world  " };
        var expected = new List<string> { "hello", "world" };

        var result = processor.ProcessWords(words);

        result.IsSuccess.Should().BeTrue("должен успешно обработать массив");
        result.Error.Should().BeNullOrEmpty("при успешной обработке ошибки отсутствуют");
        result.Value.Should().BeEquivalentTo(expected, "пробелы должны быть удалены");
    }
}
