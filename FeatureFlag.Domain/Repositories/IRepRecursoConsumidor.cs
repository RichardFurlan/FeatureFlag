using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecursoConsumidor
{
    Task<List<RecursoConsumidor>> RecuperarTodosAsync(string query);
    Task<List<RecursoConsumidor>> RecuperarTodosPorConsumidor(int id);
    Task<RecursoConsumidor> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(RecursoConsumidor recursoConsumidor);
    Task AlterarAsync(int id, RecursoConsumidor recursoConsumidor);
    Task InativarAsync(int id);
}