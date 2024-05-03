using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace QueryWatch;

public static class ServiceLocator
{
    private static IServiceProvider _provider;

    public static void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static T GetService<T>()
    {
        return _provider.GetService<T>();
    }


    public static IDbConnection GetConnection()
    {
        var factory = GetService<DbConnectionFactory>();
        return factory.CreateConnection();
    }
}
