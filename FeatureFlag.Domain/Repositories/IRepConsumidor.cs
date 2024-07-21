using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepConsumidor
{
    Task<List<Consumidor>> RecuperarTodosAsync();
    Task<Consumidor?> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(Consumidor consumidor);
    Task AlterarAsync(Consumidor consumidor);
    Task InativarAsync(Consumidor consumidor);
}