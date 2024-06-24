using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;

namespace Repository.Persistence.Repositories;

public class RepRecursoMemory : IRepRecurso
{
    private static readonly List<Recurso> Recurso = new List<Recurso>();

    public Task<List<Recurso>> ListarTodosAsync(string query)
    {
        var resultado = Recurso
            .Where(r => string.IsNullOrEmpty(query) || r.Identificacao.Contains(query) || r.Descricao.Contains(query))
            .ToList();
        return Task.FromResult(resultado);
    }

    public Task<Recurso> ListarPorIdAsync(int id)
    {
        var resultado = Recurso.SingleOrDefault(rc => rc.Id == id);
        return Task.FromResult(resultado);
    }

    public Task InserirAsync(Recurso recurso)
    {
        Recurso.Add(recurso);
        return Task.CompletedTask;
    }

    public Task AlterarAsync(int id, Recurso recurso)
    {
        var index = Recurso.FindIndex(rc => rc.Id == id);
        if (index != -1)
        {
            Recurso[index] = recurso;
        }
        return Task.CompletedTask;
    }

    public Task InativarAsync(int id)
    {
        var recurso = ListarPorIdAsync(id);
        recurso.Result.Inativar();
        return Task.CompletedTask;
    }
}