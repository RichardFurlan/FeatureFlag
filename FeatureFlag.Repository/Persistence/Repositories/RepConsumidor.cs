using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence.Repositories;

public class RepConsumidor : IRepConsumidor
{
    private readonly FeatureFlagDbContext _dbContext;

    public RepConsumidor(FeatureFlagDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Consumidor>> RecuperarTodosAsync()
    {
        var resultado = await _dbContext.Consumidores.ToListAsync();
        return resultado;
    }

    public async Task<Consumidor?> RecuperarPorIdAsync(int id)
    {
        var consumidor = await _dbContext.Consumidores.SingleOrDefaultAsync(c => c.Id == id);
        return consumidor;
    }

    public async Task<Consumidor?> RecuperarPorIdentificacaoAsync(string identificacaoConsumidor)
    {
        var consumidor = await _dbContext.Consumidores.SingleOrDefaultAsync(c => c.Identificacao == identificacaoConsumidor);
        return consumidor;
    }

    public async Task<int> InserirAsync(Consumidor consumidor)
    { 
        await _dbContext.Consumidores.AddAsync(consumidor);
        await _dbContext.SaveChangesAsync();
        return consumidor.Id;
    }

    public async Task AlterarAsync(Consumidor consumidor)
    {
        _dbContext.Consumidores.Update(consumidor);
        await _dbContext.SaveChangesAsync();
    }


    public async Task InativarAsync(Consumidor consumidor)
    {
        _dbContext.Consumidores.Update(consumidor);
        await _dbContext.SaveChangesAsync();
    }
}