using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepRecursoConsumidorMemory : IRepRecursoConsumidor
{
    private static readonly List<RecursoConsumidor> RecursosConsumidores = new List<RecursoConsumidor>();
    
    public Task<List<RecursoConsumidor>> RecuperarTodosAsync()
    {
        var resultado = RecursosConsumidores
            .ToList();
        return Task.FromResult(resultado);
    }

    public Task<List<RecursoConsumidor>> RecuperarTodosPorConsumidor(int id)
    {
        var resultado = RecursosConsumidores.FindAll(rc => rc.CodigoConsumidor == id);
        return Task.FromResult(resultado);
    }

    public Task<RecursoConsumidor> RecuperarPorIdAsync(int id)
    {
        var index = RecursosConsumidores.FindIndex(rc => rc.Id == id);
        return Task.FromResult(RecursosConsumidores[index]);
    }

    public Task<int> InserirAsync(RecursoConsumidor recursoConsumidor)
    {
        RecursosConsumidores.Add(recursoConsumidor);
        return Task.FromResult(recursoConsumidor.Id);
    }

    public async Task AlterarAsync(int id, RecursoConsumidor recursoConsumidor)
    {
        var recursoConsumidorExistente = await RecuperarPorIdAsync(id);
        if (recursoConsumidorExistente == null)
        {
            throw new KeyNotFoundException($"RecursoConsumidor com ID {id} não encontrado.");
        }
        recursoConsumidorExistente.Update(
            recursoConsumidor.CodigoRecurso,
            recursoConsumidor.CodigoConsumidor,
            recursoConsumidor.Status
            );
    }

    public Task InativarAsync(int id)
    {
        var recursoConsumidor = RecuperarPorIdAsync(id);
        recursoConsumidor.Result.Inativar();
        return Task.CompletedTask;
    }
}