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

    public async Task AlterarAsync(int id, Recurso recurso)
    {
        var recursoExistente = await RecuperarPorIdAsync(id);
        if (recursoExistente == null)
        {
            throw new KeyNotFoundException($"Recurso com ID {id} não encontrado.");
        }
        recursoExistente.Update(recurso);
    }

    public async Task InativarAsync(int id)
    {
        var recurso = await RecuperarPorIdAsync(id);
        recurso.Inativar();
    }
}