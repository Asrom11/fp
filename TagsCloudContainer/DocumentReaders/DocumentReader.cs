using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.DocumentReaders;

public class DocumentReader: IDocumentReader
{
    private readonly Dictionary<string, IDocumentReader> _readers;
    public string[] SupportedDocumentExtensions => _readers.Keys.ToArray();
    public DocumentReader(IEnumerable<IDocumentReader> readers)
    {
        _readers = readers
            .SelectMany(reader => reader.SupportedDocumentExtensions.Select(ext => new { ext, reader }))
            .ToDictionary(x => x.ext, x => x.reader);
    }
    public Result<string[]?> ReadDocument(string filePath)
    {
        if (!File.Exists(filePath))
            return Result.Fail<string[]>($"Файл не найден или недоступен: {filePath}");

        var extension = Path.GetExtension(filePath).ToLower();
        if (!_readers.TryGetValue(extension, out var reader))
            return Result.Fail<string[]>($"Формат файла {extension} не поддерживается.");

        return reader
            .ReadDocument(filePath)
            .RefineError($"Не удалось прочитать документ: {filePath}");
    }
}