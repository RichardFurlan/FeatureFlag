using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepRecurso : IRepRecurso
{
    public Task<List<Recurso>> ListarTodosAsync(string query)
    {
        throw new NotImplementedException();
    }

    public Task<Recurso> ListarPorIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task InserirAsync(Recurso recurso)
    {
        throw new NotImplementedException();
    }

    public Task AlterarAsync(int id, Recurso recurso)
    {
        throw new NotImplementedException();
    }

    public Task InativarAsync(int id)
    {
        throw new NotImplementedException();
    }
}