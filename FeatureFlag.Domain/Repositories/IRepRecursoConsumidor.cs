using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecursoConsumidor
{
    IQueryable<RecursoConsumidor> RecuperarTodos();
    Task<List<RecursoConsumidor>> RecuperarTodosPorCodigoConsumidorAsync(int codigoConsumidor);
    Task<List<RecursoConsumidor>> RecuperarTodosPorCodigoRecursoAsync(int codigoRecurso);
    Task<RecursoConsumidor?> RecuperarPorCodigoRecursoEConsumidorAsync(int codigoRecurso, int codigoConsumidor);
    Task<RecursoConsumidor?> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(RecursoConsumidor recursoConsumidor);
    Task AlterarAsync(RecursoConsumidor recursoConsumidor);
    Task InativarAsync(RecursoConsumidor recursoConsumidor);
}