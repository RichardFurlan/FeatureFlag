using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Interefaces;

namespace FeatureFlag.Application.Aplicacao;

public class AplicConsumidor : IAplicConsumidor
{
    public async Task<List<ConsumidorViewModel>> ListarTodos(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<ConsumidorViewModel> ListarPorId(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Inserir(CreateConsumidorInputModel createConsumidorInputModel)
    {
        throw new NotImplementedException();
    }

    public async Task Alterar(UpdateConsumidorInputModel updateConsumidorInputModel)
    {
        throw new NotImplementedException();
    }

    public async Task Inativar(int id)
    {
        throw new NotImplementedException();
    }
}