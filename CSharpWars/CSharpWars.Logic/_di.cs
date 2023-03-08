using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Logic;

public static class ServiceCollectionExtensions
{
    public static void AddLogic(this IServiceCollection services)
    {
        services.AddSingleton<IProcessorLogic, ProcessorLogic>();
        services.AddSingleton<IPreprocessingLogic, PreprocessingLogic>();
        services.AddSingleton<IProcessingLogic, ProcessingLogic>();
        services.AddSingleton<IPostprocessingLogic, PostprocessingLogic>();
    }
}