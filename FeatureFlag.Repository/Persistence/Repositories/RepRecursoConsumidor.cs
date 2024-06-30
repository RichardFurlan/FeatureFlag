using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepRecursoConsumidor 
{
    public Task<List<RecursoConsumidor>> ListarTodosAsync(string query)
    {
        throw new NotImplementedException();
    }

    public Task<RecursoConsumidor> ListarPorIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task InserirAsync(RecursoConsumidor recursoConsumidor)
    {
        throw new NotImplementedException();
    }

    public Task AlterarAsync(int id, RecursoConsumidor recursoConsumidor)
    {
        throw new NotImplementedException();
    }

    public Task InativarAsync(int id)
    {
        throw new NotImplementedException();
    }
}