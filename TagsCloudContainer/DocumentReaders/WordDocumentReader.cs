using DocumentFormat.OpenXml.Packaging;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.DocumentReaders;

public class WordDocumentReader : IDocumentReader
{
    public string[] SupportedDocumentExtensions => [".doc", ".docx"];

    public Result<string[]?> ReadDocument(string filePath)
    {
        using var doc = WordprocessingDocument.Open(filePath, false);
        var body = doc.MainDocumentPart?.Document.Body;
        if (body == null) return Array.Empty<string>();

        var text = body.InnerText;
        return text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    }
}