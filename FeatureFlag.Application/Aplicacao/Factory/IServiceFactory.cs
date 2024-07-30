namespace FeatureFlag.Application.Aplicacao.Factory;

public interface IServiceFactory
{
    T Create<T>();
}