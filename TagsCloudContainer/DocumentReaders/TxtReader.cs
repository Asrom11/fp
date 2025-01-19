using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.DocumentReaders;

public class TxtReader : IDocumentReader
{
    public string[] SupportedDocumentExtensions => [".txt"];

    public Result<string[]?> ReadDocument(string filePath)
    {
        return File.ReadAllLines(filePath);
    }
}