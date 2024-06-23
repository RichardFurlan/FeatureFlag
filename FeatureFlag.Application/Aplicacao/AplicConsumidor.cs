using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Interefaces;

namespace FeatureFlag.Application.Aplicacao;

public class AplicConsumidor : IAplicConsumidor
{
    public List<ConsumidorViewModel> ListarTodos(string query)
    {
        throw new NotImplementedException();
    }

    public ConsumidorViewModel ListarPorId(int id)
    {
        throw new NotImplementedException();
    }

    public int Inserir(CreateConsumidorInputModel createConsumidorInputModel)
    {
        throw new NotImplementedException();
    }

    public void Alterar(UpdateConsumidorInputModel updateConsumidorInputModel)
    {
        throw new NotImplementedException();
    }

    public void Inativar(int id)
    {
        throw new NotImplementedException();
    }
}