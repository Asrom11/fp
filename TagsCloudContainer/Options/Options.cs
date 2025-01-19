using CommandLine;

namespace TagsCloudContainer.Options;

public class Options
{
    [Option('i', "input", Required = true, HelpText = "Path to the input text file.")]
    public string InputFilePath { get; set; }

    [Option('o', "output", Required = true, HelpText = "Path to save the output image.")]
    public string OutputFilePath { get; set; }

    [Option("bgcolor", Default = "White", HelpText = "Background color (name or hex).")]
    public string BackgroundColor { get; set; }

    [Option("wordcolors", Default = "Black,Blue,Green,Red,Purple",
        HelpText = "Comma-separated list of word colors (names or hex values).")]
    public string WordColors { get; set; }

    [Option("font", Default = "Times New Roman", HelpText = "Font name for text rendering.")]
    public string Font { get; set; }

    [Option("width", Default = 1000, HelpText = "Width of the output image.")]
    public int Width { get; set; }

    [Option("height", Default = 1000, HelpText = "Height of the output image.")]
    public int Height { get; set; }

    [Option("boringwords", Default = "", HelpText = "Path to the file containing boring words.")]
    public string BoringWordsFilePath { get; set; }
}