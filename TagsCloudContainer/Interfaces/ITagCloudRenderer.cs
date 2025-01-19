using TagsCloudContainer.Options;

namespace TagsCloudContainer.Interfaces;

public interface ITagCloudRenderer
{
    Result<None> Render(IEnumerable<Tag> tags, string outputFilePath, RenderingOptions options);
}