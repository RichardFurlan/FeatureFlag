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
    public IQueryable<Consumidor> RecuperarTodos()
    {
        return _dbContext.Consumidores;
    }

    public Task<Consumidor?> RecuperarPorIdAsync(int id)
    {
        return _dbContext.Consumidores.SingleOrDefaultAsync(c => c.Id == id);
    }

    public Task<Consumidor?> RecuperarPorIdentificacaoAsync(string identificacaoConsumidor)
    {
        return _dbContext.Consumidores.SingleOrDefaultAsync(c => c.Identificacao == identificacaoConsumidor);
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