using Microsoft.Extensions.DependencyInjection;

namespace OrleansSnake.Client;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<GameClient>();
        services.AddSingleton<MainForm>();
        var serviceProvider = services.BuildServiceProvider();
        
        ApplicationConfiguration.Initialize();
        Application.Run(serviceProvider.GetService<MainForm>());
    }
}