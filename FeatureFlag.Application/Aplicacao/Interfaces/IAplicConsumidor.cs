using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Domain.Interefaces;

public interface IAplicConsumidor
{
    List<ConsumidorViewModel> ListarTodos(string query);
    ConsumidorViewModel ListarPorId(int id);
    int Inserir(CreateConsumidorInputModel createConsumidorInputModel);
    void Alterar(UpdateConsumidorInputModel updateConsumidorInputModel);
    void Inativar(int id);
}