using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepConsumidor
{
    IQueryable<Consumidor> RecuperarTodos();
    Task<Consumidor?> RecuperarPorIdAsync(int id);
    Task<Consumidor?> RecuperarPorIdentificacaoAsync(string identificacaoConsumidor);
    Task<int> InserirAsync(Consumidor consumidor);
    Task AlterarAsync(Consumidor consumidor);
    Task InativarAsync(Consumidor consumidor);
}