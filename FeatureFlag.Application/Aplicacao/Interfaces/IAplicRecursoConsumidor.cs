using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao.Interfaces;

public interface IAplicRecursoConsumidor
{
    List<RecursoConsumidorViewModel> ListarTodos(string query);
    ConsumidorViewModel ListarPorId(int id);
    int Inserir(CreateRecursoConsumidorInputModel createConsumidorInputModel);
    void Alterar(UpdateRecursoConsumidorInputModel updateConsumidorInputModel);
    void Inativar(int id);
}