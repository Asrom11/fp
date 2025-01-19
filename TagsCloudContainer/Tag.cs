using System.Drawing;

namespace TagsCloudContainer;

public class Tag(string word, int frequency)
{
    public string Word { get; } = word;
    public int Frequency { get; } = frequency;
    public Rectangle Rectangle { get; set; }
}