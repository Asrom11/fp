using TagsCloudContainer.Options;

namespace TagsCloudContainer.Interfaces;

public interface ITagCloudGenerator
{
    Result<None> GenerateCloud(string inputFilePath, string outputFilePath, RenderingOptions options);
}