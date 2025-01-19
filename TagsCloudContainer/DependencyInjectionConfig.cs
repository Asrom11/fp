using System.Drawing;
using Autofac;
using TagsCloudContainer.DocumentReaders;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Options;
using TagsCloudContainer.PointGenerators;

namespace TagsCloudContainer;

public static class DependencyInjectionConfig
{
    public static IContainer BuildContainer(Options.Options? options = null)
    {
        var builder = new ContainerBuilder();

        builder.Register(c => new RenderingOptions())
            .AsSelf()
            .SingleInstance();

        var boringWords = "";
        if (options is not null)
        {
            boringWords = options.BoringWordsFilePath;
        }

        builder.RegisterType<WordProcessor>()
            .As<IWordProcessor>()
            .SingleInstance()
            .WithParameter("filepath",
                boringWords);

        builder.RegisterType<WordDocumentReader>().As<IDocumentReader>().SingleInstance();
        builder.RegisterType<TxtReader>().As<IDocumentReader>().SingleInstance();

        builder.RegisterType<DocumentReader>().AsSelf().SingleInstance();

        builder.RegisterType<LogarithmicScaling>().As<ITextSizeCalculator>().SingleInstance();

        builder.RegisterType<WordFrequencyAnalyzer>()
            .As<IWordFrequencyAnalyzer>()
            .SingleInstance();

        builder.RegisterType<CircularCloudLayouter>()
            .As<ITagCloudLayouter>()
            .SingleInstance();

        builder.RegisterType<SpiralGenerator>()
            .As<IPointGenerator>()
            .SingleInstance()
            .WithParameter("center", new Point(500, 500));

        builder.RegisterType<LinearSpiral>()
            .Named<IPointGenerator>("linear");

        builder.RegisterType<CloudImageRenderer>()
            .As<ITagCloudRenderer>()
            .SingleInstance();

        builder.RegisterType<TagCloudGenerator>()
            .As<ITagCloudGenerator>()
            .SingleInstance();

        builder.Register<ITagCloudGenerator>(c =>
                new TagCloudGenerator(
                    c.Resolve<IWordProcessor>(),
                    c.Resolve<ITagCloudLayouter>(),
                    c.Resolve<ITagCloudRenderer>(),
                    c.Resolve<DocumentReader>(),
                    c.Resolve<IWordFrequencyAnalyzer>(),
                    c.Resolve<ITextSizeCalculator>()
                ))
            .SingleInstance();

        return builder.Build();
    }
}