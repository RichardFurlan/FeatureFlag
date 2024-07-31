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
    
    public async Task<List<RecursoConsumidor>> RecuperarTodosAsync()
    {
        var resultado = await _dbContext.RecursosConsumidores.ToListAsync();
        return resultado;
    }

    public async Task<List<RecursoConsumidor>> RecuperarTodosPorCodigoConsumidorAsync(int codigoConsumidor)
    {
        var resultado = await _dbContext.RecursosConsumidores.Where(rc => rc.CodigoConsumidor.Equals(codigoConsumidor)).ToListAsync();
        return resultado;
    }
    
    public async Task<List<RecursoConsumidor>> RecuperarTodosPorCodigoRecursoAsync(int codigoRecurso)
    {
        var resultado = await _dbContext.RecursosConsumidores.Where(rc => rc.CodigoRecurso.Equals(codigoRecurso)).ToListAsync();
        return resultado;
    }
    
    public async Task<RecursoConsumidor?> RecuperarPorCodigoRecursoEConsumidorAsync(int codigoRecurso, int codigoConsumidor)
    {
        var resultado = await _dbContext.RecursosConsumidores
            .SingleOrDefaultAsync(rc => rc.CodigoRecurso == codigoRecurso && rc.CodigoConsumidor == codigoConsumidor);
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