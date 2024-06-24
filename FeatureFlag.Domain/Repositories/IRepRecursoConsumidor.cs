using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecursoConsumidor
{
    Task<List<RecursoConsumidor>> ListarTodosAsync(string query);
    Task<RecursoConsumidor> ListarPorIdAsync(int id);
    Task InserirAsync(RecursoConsumidor recursoConsumidor);
    Task AlterarAsync(int id, RecursoConsumidor recursoConsumidor);
    Task InativarAsync(int id);
}