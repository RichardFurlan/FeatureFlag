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

    public IQueryable<Recurso> RecuperarTodos()
    {
        return _dbContext.Recursos;
    }

    public Task<Recurso?> RecuperarPorIdAsync(int id)
    {
        return _dbContext.Recursos.SingleOrDefaultAsync(r => r.Id == id);
    }

    public Task<Recurso?> RecuperarPorIdentificacaoAsync(string identificacaoRecurso)
    {
        return _dbContext.Recursos.SingleOrDefaultAsync(r => r.Identificacao == identificacaoRecurso);
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