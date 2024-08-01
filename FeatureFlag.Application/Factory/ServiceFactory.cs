using Microsoft.Extensions.DependencyInjection;

namespace FeatureFlag.Application.Factory;

public class ServiceFactory : IServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T Create<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}