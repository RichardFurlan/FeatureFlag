using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepConsumidor : IRepConsumidor
{
    public Task<List<Consumidor>> ListarTodosAsync(string query)
    {
        throw new NotImplementedException();
    }

    public Task<Consumidor> ListarPorIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task InserirAsync(Consumidor consumidor)
    {
        throw new NotImplementedException();
    }

    public Task AlterarAsync(int id, Consumidor consumidor)
    {
        throw new NotImplementedException();
    }

    public Task InativarAsync(int id)
    {
        throw new NotImplementedException();
    }
}