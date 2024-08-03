using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Domain.Repositories;

public interface IRepRecurso
{
    IQueryable<Recurso> RecuperarTodos();
    Task<Recurso?> RecuperarPorIdAsync(int id);
    Task<Recurso?> RecuperarPorIdentificacaoAsync(string identificacaoRecurso);
    Task<int> InserirAsync(Recurso recurso);
    Task AlterarAsync(Recurso recurso);
    Task InativarAsync(Recurso recurso);
}