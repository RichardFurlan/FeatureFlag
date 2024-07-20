using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence.Repositories;

public class RepRecursoMemory : IRepRecurso
{
    private static readonly List<Recurso> Recurso = new List<Recurso>();
    private readonly FeatureFlagDbContext _dbContext;

    public RepRecursoMemory(FeatureFlagDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Recurso>> RecuperarTodosAsync()
    {
        var resultado = await _dbContext.Recursos.ToListAsync();
        return resultado;
    }

    public async Task<Recurso?> RecuperarPorIdAsync(int id)
    {
        var recurso = await _dbContext.Recursos.SingleOrDefaultAsync(r => r.Id == id);
        return recurso;
    }

    public async Task<int> InserirAsync(Recurso recurso)
    {
        await _dbContext.Recursos.AddAsync(recurso);
        await _dbContext.SaveChangesAsync();
        return recurso.Id;
    }

    public async Task AlterarAsync(Recurso recurso)
    {
        _dbContext.Recursos.Update(recurso);
        await _dbContext.SaveChangesAsync();
    }

    public async Task InativarAsync(Recurso recurso)
    {
        _dbContext.Recursos.Update(recurso);
        await _dbContext.SaveChangesAsync();
    }
}