using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepConsumidor
{
    Task<List<Consumidor>> RecuperarTodosAsync();
    Task<Consumidor?> RecuperarPorIdAsync(int id);
    Task<Consumidor?> RecuperarPorIdentificacaoAsync(string identificacaoConsumidor);
    Task<int> InserirAsync(Consumidor consumidor);
    Task AlterarAsync(Consumidor consumidor);
    Task InativarAsync(Consumidor consumidor);
}