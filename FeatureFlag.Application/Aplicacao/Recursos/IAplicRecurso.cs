using FeatureFlag.Application.Aplicacao.Recursos.DTOs;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao;

public interface IAplicRecurso
{
    Task<List<RecuperarRecursoDto>> RecuperarTodosAsync(string query);
    Task<RecuperarRecursoDto> RecuperarPorIdAsync(int id);
    Task<RecuperarRecursoAtivoDto> VerificaRecursoAtivoParaConsumidorAsync(int recursoId, int cunsumidorId);
    Task<int> InserirAsync(CriarRecursoDto criarRecursoDto);
    Task<int> InserirRecursoELiberacaoAsync(CriarRecursoELiberacaoDto criarRecursoELiberacaoDto);

    Task AlterarPercentualDeLiberacaoDeRecurso(
        AlterarPercentualDeLiberacaoRecursoDto alterarPercentualDeLiberacaoRecursoDto);
    Task AlterarAsync(int id, AlterarRecursoDto alterarRecursoDto);
    Task InativarAsync(int id);
}