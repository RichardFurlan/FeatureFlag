using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepConsumidor
{
    Task<List<Consumidor>> ListarTodosAsync(string query);
    Task<Consumidor> ListarPorIdAsync(int id);
    Task InserirAsync(Consumidor consumidor);
    Task AlterarAsync(int id, Consumidor consumidor);
    Task InativarAsync(int id);
}