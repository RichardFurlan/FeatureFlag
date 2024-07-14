using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepConsumidorMemory : IRepConsumidor
{
    private static readonly List<Consumidor> Consumidores = new List<Consumidor>();
    public Task<List<Consumidor>> RecuperarTodosAsync()
    {
        var resultado = Consumidores
            .ToList();
        
        return Task.FromResult(resultado);
    }

    public Task<Consumidor> RecuperarPorIdAsync(int id)
    {
        var index = Consumidores.FindIndex(c => c.Id == id);
        return Task.FromResult(Consumidores[index]);
    }

    public Task<int> InserirAsync(Consumidor consumidor)
    { 
        Consumidores.Add(consumidor);
        return Task.FromResult(consumidor.Id);
    }

    public async Task AlterarAsync(int id, Consumidor consumidor)
    {
        var consumidorExistente = await RecuperarPorIdAsync(id);
        if (consumidorExistente == null)
        {
            throw new KeyNotFoundException($"Consumidor com ID {id} não encontrado.");
        }
        consumidorExistente.Update(consumidor);
    }


    public Task InativarAsync(int id)
    {
        var consumidor = RecuperarPorIdAsync(id);
        consumidor.Result.Inativar();
        return Task.CompletedTask;
    }
}