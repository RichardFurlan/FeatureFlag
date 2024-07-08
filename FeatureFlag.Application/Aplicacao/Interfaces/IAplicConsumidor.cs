using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Domain.Interefaces;

public interface IAplicConsumidor
{
    Task<List<ConsumidorViewModel>> ListarTodos();
    Task<ConsumidorViewModel> ListarPorId(int id);
    Task<int> Inserir(CreateConsumidorInputModel createConsumidorInputModel);
    Task<RecursosPorConsumidorViewModel> RecuperaRecursosPorConsumidor(int id);
    Task Alterar(int id, UpdateConsumidorInputModel updateConsumidorInputModel);
    Task Inativar(int id);
}