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

    public Task<List<RecursoConsumidor>> ListarTodosPorConsumidor(int id)
    {
        var resultado = RecursosConsumidores.FindAll(rc => rc.CodigoConsumidor == id);
        return Task.FromResult(resultado);
    }

    public Task<RecursoConsumidor> ListarPorIdAsync(int id)
    {
        var index = RecursosConsumidores.FindIndex(rc => rc.Id == id);
        return Task.FromResult(RecursosConsumidores[index]);
    }

    public Task<int> InserirAsync(RecursoConsumidor recursoConsumidor)
    {
        RecursosConsumidores.Add(recursoConsumidor);
        return Task.FromResult(recursoConsumidor.Id);
    }

    public Task AlterarAsync(int id, RecursoConsumidor recursoConsumidor)
    {
        throw new NotImplementedException();
    }

    public Task InativarAsync(int id)
    {
        var recursoConsumidor = ListarPorIdAsync(id);
        recursoConsumidor.Result.Inativar();
        return Task.CompletedTask;
    }
}