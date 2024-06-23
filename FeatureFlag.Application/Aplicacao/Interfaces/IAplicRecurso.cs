using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao;

public interface IAplicRecurso
{
    List<RecursoViewModel> ListarTodos(string query);
    RecursoViewModel ListarPorId(int id);
    int Inserir(CreateRecursoInputModel createRecursoInputModel);
    void Alterar(UpdateRecursoInputModel updateRecursoInputModel);
    void Inativar(int id);
}