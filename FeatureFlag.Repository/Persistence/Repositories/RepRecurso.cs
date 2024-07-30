using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence.Repositories;

public class RepRecurso : IRepRecurso
{
    private readonly FeatureFlagDbContext _dbContext;

    public RepRecurso(FeatureFlagDbContext dbContext)
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

    public async Task<Recurso?> RecuperarPorIdentificacaoAsync(string identificacaoRecurso)
    {
        var recurso = await _dbContext.Recursos.SingleOrDefaultAsync(r => r.Identificacao == identificacaoRecurso);
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