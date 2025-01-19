using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Options;

namespace TagsCloudContainer;

public class TagCloudGenerator(
    IWordProcessor wordProcessor,
    ITagCloudLayouter layouter,
    ITagCloudRenderer renderer,
    IDocumentReader textReader,
    IWordFrequencyAnalyzer analyzer,
    ITextSizeCalculator sizeCalculator
)
    : ITagCloudGenerator
{
    public Result<None> GenerateCloud(string inputFilePath, string outputFilePath, RenderingOptions options)
    {
        var wordsResult = textReader.ReadDocument(inputFilePath);
        if (!wordsResult.IsSuccess)
            return Result.Fail<None>(wordsResult.Error);

        var processedWordsResult = wordProcessor.ProcessWords(wordsResult.Value);
        if (!processedWordsResult.IsSuccess)
        {
            return Result.Fail<None>(processedWordsResult.Error);
        }

        var tags = analyzer.Analyze(processedWordsResult.Value);
        foreach (var tag in tags)
        {
            var size = sizeCalculator.GetWordSize(tag.Word, tag.Frequency, options);
            var rectResult = layouter.PutNextRectangle(size);
            if (!rectResult.IsSuccess)
                return Result.Fail<None>(rectResult.Error);

            tag.Rectangle = rectResult.Value;
        }

        var renderResult = renderer.Render(tags, outputFilePath, options);
        return !renderResult.IsSuccess
            ? Result.Fail<None>(renderResult.Error)
            : Result.Ok();
    }
}