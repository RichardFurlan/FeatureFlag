using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecursoConsumidor
{
    Task<List<RecursoConsumidor>> RecuperarTodosAsync();
    Task<List<RecursoConsumidor>> RecuperarTodosPorConsumidorAsync(int id);
    Task<RecursoConsumidor> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(RecursoConsumidor recursoConsumidor);
    Task AlterarAsync(RecursoConsumidor recursoConsumidor);
    Task InativarAsync(RecursoConsumidor recursoConsumidor);
}