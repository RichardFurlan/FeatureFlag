using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.Recursos.DTOs;

namespace FeatureFlag.Application.Recursos;

public interface IAplicRecurso
{
    Task<List<RecuperarRecursoView>> RecuperarTodosAsync();
    Task<RecuperarRecursoView> RecuperarPorIdAsync(int id);
    Task<RecuperarRecursoAtivoView> VerificaRecursoAtivoParaConsumidorIdentificacaoAsync(string identificacaoRecurso,
        string identificacaoConsumidor);
    Task<int> InserirAsync(CriarRecursoDTO criarRecursoDto);
    Task AlterarPercentualDeLiberacaoDeRecurso(
        AlterarPercentualDeLiberacaoRecursoDto alterarPercentualDeLiberacaoRecursoDto);
    Task AlterarAsync(int id, AlterarRecursoDTO alterarRecursoDto);
    Task InativarAsync(int id);
}