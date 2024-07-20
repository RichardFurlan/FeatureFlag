using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence.Repositories;

public class RepRecursoConsumidorMemory : IRepRecursoConsumidor
{
    private readonly FeatureFlagDbContext _dbContext;
    private static readonly List<RecursoConsumidor> RecursosConsumidores = new List<RecursoConsumidor>();

    public RepRecursoConsumidorMemory(FeatureFlagDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<RecursoConsumidor>> RecuperarTodosAsync()
    {
        var resultado = await _dbContext.RecursosConsumidores.ToListAsync();
        return await Task.FromResult(resultado);
    }

    public async Task<List<RecursoConsumidor>> RecuperarTodosPorConsumidorAsync(int id)
    {
        var resultado = await _dbContext.RecursosConsumidores.Where(rc => rc.CodigoConsumidor.Equals(id)).ToListAsync();
        return await Task.FromResult(resultado);
    }

    public async Task<RecursoConsumidor?> RecuperarPorIdAsync(int id)
    {
        var recursoConsumidor = await _dbContext.RecursosConsumidores.FindAsync(id);
        return await Task.FromResult(recursoConsumidor);
    }

    public async Task<int> InserirAsync(RecursoConsumidor recursoConsumidor)
    {
        _dbContext.RecursosConsumidores.Add(recursoConsumidor);
        await _dbContext.SaveChangesAsync();
        return await Task.FromResult(recursoConsumidor.Id);
    }

    public async Task AlterarAsync(RecursoConsumidor recursoConsumidor)
    {
        _dbContext.Update(recursoConsumidor);
        await _dbContext.SaveChangesAsync();
    }

    public async Task InativarAsync(RecursoConsumidor recursoConsumidor)
    {
        _dbContext.Update(recursoConsumidor);
        await _dbContext.SaveChangesAsync();
    }
}