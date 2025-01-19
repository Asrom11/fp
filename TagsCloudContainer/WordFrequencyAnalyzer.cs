using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class WordFrequencyAnalyzer: IWordFrequencyAnalyzer
{
    public List<Tag> Analyze(IEnumerable<string> words)
    {
        var wordFrequencies = words
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());

        return wordFrequencies
            .OrderByDescending(pair => pair.Value)
            .Select(pair => new Tag(pair.Key, pair.Value))
            .ToList();
    }
}