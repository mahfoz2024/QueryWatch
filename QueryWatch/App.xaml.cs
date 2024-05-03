using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace QueryWatch;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {

        base.OnStartup(e);
        var services = new ServiceCollection();
        services.AddSingleton<DbConnectionFactory>(
            new DbConnectionFactory("Server=172.30.0.165;Database=mobitrans;User Id=Mahfouz;Password=Aaa@1234;Trusted_Connection=False;Encrypt=false"));
        var serviceProvider = services.BuildServiceProvider();
        ServiceLocator.SetProvider(serviceProvider);
    }
}
