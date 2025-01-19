namespace TagsCloudContainer.Interfaces;

public interface IWordFrequencyAnalyzer
{
    List<Tag> Analyze(IEnumerable<string> words);
}