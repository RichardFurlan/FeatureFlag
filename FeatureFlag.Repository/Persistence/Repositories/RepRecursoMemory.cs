using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepRecursoMemory : IRepRecurso
{
    private static readonly List<Recurso> Recurso = new List<Recurso>();

    public Task<List<Recurso>> RecuperarTodosAsync()
    {
        var resultado = Recurso
            .ToList();
        return Task.FromResult(resultado);
    }

    public Task<Recurso> RecuperarPorIdAsync(int id)
    {
        var index = Recurso.FindIndex(r => r.Id == id);
        return Task.FromResult(Recurso[index]);
    }

    public Task<int> InserirAsync(Recurso recurso)
    {
        Recurso.Add(recurso);
        return Task.FromResult(recurso.Id);
    }

    public Task AlterarAsync(int id, Recurso recurso)
    {
        throw new NotImplementedException();
    }

    public Task InativarAsync(int id)
    {
        var recurso = RecuperarPorIdAsync(id);
        recurso.Result.Inativar();
        return Task.CompletedTask;
    }
}