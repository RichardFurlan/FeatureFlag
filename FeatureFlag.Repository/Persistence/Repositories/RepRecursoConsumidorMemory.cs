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
        return resultado;
    }

    public async Task<List<RecursoConsumidor>> RecuperarTodosPorConsumidorAsync(int id)
    {
        var resultado = await _dbContext.RecursosConsumidores.Where(rc => rc.CodigoConsumidor.Equals(id)).ToListAsync();
        return resultado;
    }

    public async Task<RecursoConsumidor?> RecuperarPorIdAsync(int id)
    {
        var recursoConsumidor = await _dbContext.RecursosConsumidores.SingleOrDefaultAsync(rc => rc.Id == id);
        return recursoConsumidor;
    }

    public async Task<int> InserirAsync(RecursoConsumidor recursoConsumidor)
    {
        await _dbContext.RecursosConsumidores.AddAsync(recursoConsumidor);
        await _dbContext.SaveChangesAsync();
        return recursoConsumidor.Id;
    }

    public async Task AlterarAsync(RecursoConsumidor recursoConsumidor)
    {
        _dbContext.RecursosConsumidores.Update(recursoConsumidor);
        await _dbContext.SaveChangesAsync();
    }

    public async Task InativarAsync(RecursoConsumidor recursoConsumidor)
    {
        _dbContext.RecursosConsumidores.Update(recursoConsumidor);
        await _dbContext.SaveChangesAsync();
    }
}