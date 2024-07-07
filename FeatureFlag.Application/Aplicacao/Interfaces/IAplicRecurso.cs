using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao;

public interface IAplicRecurso
{
    Task<List<RecursoViewModel>> ListarTodos(string query);
    Task<RecursoViewModel> ListarPorId(int id);
    Task<RecursoAtivoViewModel> VerificaRecurso(int recursoId, int cunsumidorId);
    Task<int> Inserir(CreateRecursoInputModel createRecursoInputModel);
    Task<int> InserirRecursoELiberacao(CreateRecursoELiberacaoInputModel createRecursoELiberacaoInputModel);
    Task Alterar(int id, UpdateRecursoInputModel updateRecursoInputModel);
    Task Inativar(int id);
}