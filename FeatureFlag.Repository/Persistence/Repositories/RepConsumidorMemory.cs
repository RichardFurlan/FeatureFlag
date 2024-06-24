using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepConsumidorMemory : IRepConsumidor
{
    private static readonly List<Consumidor> Consumidores = new List<Consumidor>();
    public Task<List<Consumidor>> ListarTodosAsync(string query)
    {
        var resultado = Consumidores
            .Where(c => string.IsNullOrEmpty(query) || c.Identificacao.Contains(query) || c.Descricao.Contains(query))
            .ToList();
        
        return Task.FromResult(resultado);
    }

    public Task<Consumidor> ListarPorIdAsync(int id)
    {
        var consumidor = Consumidores.SingleOrDefault(c => c.Id == id);
        return Task.FromResult(consumidor);
    }

    public Task InserirAsync(Consumidor consumidor)
    { 
        Consumidores.Add(consumidor);
        return Task.CompletedTask;
    }

    public Task AlterarAsync(int id, Consumidor consumidor)
    {
        var index = Consumidores.FindIndex(c => c.Id == id);
        if (index != -1)
        {
            Consumidores[index] = consumidor;
        }
        return Task.CompletedTask;
    }

    public Task InativarAsync(int id)
    {
        var consumidor = ListarPorIdAsync(id);
        consumidor.Result.Inativar();
        return Task.CompletedTask;
    }
}