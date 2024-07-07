using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao.Interfaces;

public interface IAplicRecursoConsumidor
{
    Task<List<RecursoConsumidorViewModel>> ListarTodos(string query);
    Task<RecursoConsumidorViewModel> ListarPorId(int id);
    Task<int> Inserir(CreateRecursoConsumidorInputModel createConsumidorInputModel);
    Task Alterar(int id, UpdateRecursoConsumidorInputModel updateConsumidorInputModel);
    Task Inativar(int id);
}