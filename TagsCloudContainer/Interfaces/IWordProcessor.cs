namespace TagsCloudContainer.Interfaces;

public interface IWordProcessor
{
    Result<IEnumerable<string>> ProcessWords(IEnumerable<string>? words);
}