namespace FeatureFlag.Application.Factory;

public interface IServiceFactory
{
    T Create<T>();
}