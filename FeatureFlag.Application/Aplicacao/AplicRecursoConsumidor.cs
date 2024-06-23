using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Interefaces;

namespace FeatureFlag.Application.Aplicacao;

public class AplicRecursoConsumidor : IAplicRecursoConsumidor
{
    public async Task<List<RecursoConsumidorViewModel>> ListarTodos(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<ConsumidorViewModel> ListarPorId(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Inserir(CreateRecursoConsumidorInputModel createConsumidorInputModel)
    {
        throw new NotImplementedException();
    }

    public async Task Alterar(UpdateRecursoConsumidorInputModel updateConsumidorInputModel)
    {
        throw new NotImplementedException();
    }

    public async Task Inativar(int id)
    {
        throw new NotImplementedException();
    }
}