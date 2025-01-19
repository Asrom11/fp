using System.Drawing;
using System.Drawing.Text;
using Autofac;
using CommandLine;
using TagsCloudContainer;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Options;

public static class Program
{
    public static void Main(string[] args)
    {
        var optionsResult = Parser.Default.ParseArguments<Options>(args)
            .MapResult(
                (Options opts) => Result.Ok<Options>(opts),
                errs => Result.Fail<Options>(string.Join(Environment.NewLine, errs))
            );
        if (!optionsResult.IsSuccess)
        {
            Console.WriteLine($"Ошибка в аргументах командной строки: {optionsResult.Error}");
            return;
        }

        var options = optionsResult.Value;
        var containerResult = Result.Of(() => DependencyInjectionConfig.BuildContainer(options),
            "Ошибка при настройке контейнера зависимостей.");
        if (containerResult.IsSuccess)
        {
            Console.WriteLine(containerResult.Error);
            return;
        }

        var container = containerResult.Value;
        using var scope = container.BeginLifetimeScope();
        var generatorResult = Result.Of(() =>
            scope.Resolve<ITagCloudGenerator>(),
            "Ошибка при разрешении зависимости ITagCloudGenerator.");
        if (generatorResult.IsSuccess)
        {
            Console.WriteLine(generatorResult.Error);
            return;
        }

        var renderingOptionsResult = ParseRenderingOptions(options);
        if (!renderingOptionsResult.IsSuccess)
        {
            Console.WriteLine($"Ошибка в настройках: {renderingOptionsResult.Error}");
            return;
        }

        var tagCloudGenerator = generatorResult.Value;
        var generateResult = tagCloudGenerator.GenerateCloud(
            options.InputFilePath,
            options.OutputFilePath,
            renderingOptionsResult.Value);

        Console.WriteLine(!generateResult.IsSuccess
            ? $"Ошибка при генерации облака тегов: {generateResult.Error}"
            : "Облако тегов успешно сгенерировано!");
    }

     private static Result<RenderingOptions> ParseRenderingOptions(Options options)
    {
        var backgroundColorResult = ParseColor(options.BackgroundColor)
            .RefineError($"Некорректный цвет для фона ({options.BackgroundColor})");

        return backgroundColorResult.Then(backgroundColor =>
        {
            var wordColorsArrayResult = options.WordColors
                .Split(',')
                .Select(ParseColor)
                .ToArray();

            var firstFail = wordColorsArrayResult.FirstOrDefault(r => !r.IsSuccess);
            if (!firstFail.IsSuccess)
                return Result.Fail<RenderingOptions>(firstFail.Error);

            var wordColors = wordColorsArrayResult.Select(r => r.Value).ToArray();

            var fontCheckResult = ValidateFontName(options.Font);
            if (!fontCheckResult.IsSuccess)
                return Result.Fail<RenderingOptions>(fontCheckResult.Error);

            var renderingOptions = new RenderingOptions
            {
                BackgroundColor = backgroundColor,
                WordColors = wordColors,
                Font = options.Font,
                ImageSize = new Size(options.Width, options.Height)
            };
            return renderingOptions;
        });
    }

    private static Result<Color> ParseColor(string colorString)
    {
        return Result.Of(() =>
        {
            if (Enum.TryParse(typeof(KnownColor), colorString, true, out var knownColorObj))
            {
                var knownColor = (KnownColor)knownColorObj!;
                return Color.FromKnownColor(knownColor);
            }

            var withHash = colorString.StartsWith("#") ? colorString : $"#{colorString}";
            return ColorTranslator.FromHtml(withHash);
        }, $"Invalid color format: {colorString}. Use a valid name (e.g., White) or HEX (e.g., #FFFFFF).\");");
    }

    public static Result<string> ValidateFontName(string fontName)
    {
        return Result.Of(() =>
            {
                using var fontsCollection = new InstalledFontCollection();
                var installedFamilies = fontsCollection.Families
                    .Select(family => family.Name.ToLowerInvariant())
                    .ToHashSet();

                if (!installedFamilies.Contains(fontName.ToLowerInvariant()))
                    throw new ArgumentException($"Шрифт '{fontName}' не найден в системе.");

                return fontName;
            }, $"Не удалось проверить шрифт '{fontName}'.");
    }
}