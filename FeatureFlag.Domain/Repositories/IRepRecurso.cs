using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecurso
{
    Task<List<Recurso>> ListarTodosAsync();
    Task<Recurso> ListarPorIdAsync(int id);
    Task<int> InserirAsync(Recurso recurso);
    Task AlterarAsync(int id, Recurso recurso);
    Task InativarAsync(int id);
}