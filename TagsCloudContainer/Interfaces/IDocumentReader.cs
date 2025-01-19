namespace TagsCloudContainer.Interfaces;

public interface IDocumentReader
{
    string[] SupportedDocumentExtensions { get; }
    Result<string[]?> ReadDocument(string filePath);
}