using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class WordProcessor: IWordProcessor
{
    private readonly HashSet<string> _boringWords;

    public WordProcessor(string filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
        {
            _boringWords = [];
            return;
        }

        var words = File.ReadAllLines(filepath);
        _boringWords = [..words];
    }
    public Result<IEnumerable<string>> ProcessWords(IEnumerable<string>? words)
    {
        if (words is null)
        {
            return Result.Fail<IEnumerable<string>>(new ArgumentNullException(nameof(words)).Message);
        }

        return Result.Ok(words
            .Select(word => word.ToLower().Trim())
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !_boringWords.Contains(word)));
    }
}
