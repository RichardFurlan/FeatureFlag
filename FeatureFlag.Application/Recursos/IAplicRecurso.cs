using FeatureFlag.Application.Aplicacao.Recursos.DTOs;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao;

public interface IAplicRecurso
{
    Task<List<RecuperarRecursoDTO>> RecuperarTodosAsync();
    Task<RecuperarRecursoDTO> RecuperarPorIdAsync(int id);
    Task<RecuperarRecursoAtivoDTO> VerificaRecursoAtivoParaConsumidorIdentificacaoAsync(string identificacaoRecurso,
        string identificacaoConsumidor);
    Task<int> InserirAsync(CriarRecursoDTO criarRecursoDto);
    Task AlterarPercentualDeLiberacaoDeRecurso(
        AlterarPercentualDeLiberacaoRecursoDto alterarPercentualDeLiberacaoRecursoDto);
    Task AlterarAsync(int id, AlterarRecursoDTO alterarRecursoDto);
    Task InativarAsync(int id);
}