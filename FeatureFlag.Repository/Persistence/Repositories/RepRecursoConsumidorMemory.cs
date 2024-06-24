using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepRecursoConsumidorMemory : IRepRecursoConsumidor
{
    private static readonly List<RecursoConsumidor> RecursosConsumidores = new List<RecursoConsumidor>();
    
    public Task<List<RecursoConsumidor>> ListarTodosAsync(string query)
    {
        var resultado = RecursosConsumidores
            .ToList();
        return Task.FromResult(resultado);
    }

    public Task<RecursoConsumidor> ListarPorIdAsync(int id)
    {
        var resultado = RecursosConsumidores.SingleOrDefault(rc => rc.Id == id);
        return Task.FromResult(resultado);
    }

    public Task InserirAsync(RecursoConsumidor recursoConsumidor)
    {
        RecursosConsumidores.Add(recursoConsumidor);
        return Task.CompletedTask;
    }

    public Task AlterarAsync(int id, RecursoConsumidor recursoConsumidor)
    {
        var index = RecursosConsumidores.FindIndex(rc => rc.Id == id);
        if (index != -1)
        {
            RecursosConsumidores[index] = recursoConsumidor;
        }
        return Task.CompletedTask;
    }

    public Task InativarAsync(int id)
    {
        var recursoConsumidor = ListarPorIdAsync(id);
        recursoConsumidor.Result.Inativar();
        return Task.CompletedTask;
    }
}