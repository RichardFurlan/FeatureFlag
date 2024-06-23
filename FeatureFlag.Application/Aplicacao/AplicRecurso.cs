using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Interefaces;

namespace FeatureFlag.Application.Aplicacao;

public class AplicRecurso : IAplicRecurso   
{
    public async Task<List<RecursoViewModel>> ListarTodos(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<RecursoViewModel> ListarPorId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> Inserir(CreateRecursoInputModel createRecursoInputModel)
    {
        throw new NotImplementedException();
    }

    public async Task Alterar(UpdateRecursoInputModel updateRecursoInputModel)
    {
        throw new NotImplementedException();
    }

    public async Task Inativar(int id)
    {
        throw new NotImplementedException();
    }
}