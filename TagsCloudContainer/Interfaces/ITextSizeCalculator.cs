using System.Drawing;
using TagsCloudContainer.Options;

namespace TagsCloudContainer.Interfaces;

public interface ITextSizeCalculator
{
    Size GetWordSize(string word, int frequency, RenderingOptions options);
}