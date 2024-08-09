using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence.Repositories;

public class RepRecursoConsumidor : IRepRecursoConsumidor
{
    private readonly FeatureFlagDbContext _dbContext;

    public RepRecursoConsumidor(FeatureFlagDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<RecursoConsumidor> RecuperarTodos()
    {
        return _dbContext.RecursosConsumidores;
    }

    public Task<List<RecursoConsumidor>> RecuperarTodosPorCodigoConsumidorAsync(int codigoConsumidor)
    {
        return _dbContext.RecursosConsumidores.Where(rc => rc.CodigoConsumidor.Equals(codigoConsumidor)).ToListAsync();
    }
    
    public Task<List<RecursoConsumidor>> RecuperarTodosPorCodigoRecursoAsync(int codigoRecurso)
    {
        return _dbContext.RecursosConsumidores.Where(rc => rc.CodigoRecurso.Equals(codigoRecurso)).ToListAsync();
    }
    
    public Task<RecursoConsumidor?> RecuperarPorCodigoRecursoEConsumidorAsync(int codigoRecurso, int codigoConsumidor)
    {
        return _dbContext.RecursosConsumidores
            .SingleOrDefaultAsync(rc => rc.CodigoRecurso == codigoRecurso && rc.CodigoConsumidor == codigoConsumidor);
    }

    public Task<RecursoConsumidor?> RecuperarPorIdAsync(int id)
    {
        return _dbContext.RecursosConsumidores.SingleOrDefaultAsync(rc => rc.Id == id);
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