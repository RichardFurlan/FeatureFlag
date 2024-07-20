using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecurso
{
    Task<List<Recurso>> RecuperarTodosAsync();
    Task<Recurso?> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(Recurso recurso);
    Task AlterarAsync(Recurso recurso);
    Task InativarAsync(Recurso recurso);
}