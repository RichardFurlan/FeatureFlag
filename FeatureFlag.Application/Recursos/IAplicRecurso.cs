using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.Recursos.DTOs;

namespace FeatureFlag.Application.Recursos;

public interface IAplicRecurso
{
    List<RecuperarRecursoView> RecuperarTodos();
    Task<RecuperarRecursoView> RecuperarPorIdAsync(int id);
    Task<RecuperarRecursoAtivoView> VerificaRecursoHabilitado(string identificacaoRecurso,
        string identificacaoConsumidor);
    Task<int> InserirAsync(CriarRecursoDTO criarRecursoDto);
    Task AlterarPercentualDeLiberacaoDeRecurso(
        AlterarPercentualDeLiberacaoRecursoDto alterarPercentualDeLiberacaoRecursoDto);
    Task AlterarAsync(int id, AlterarRecursoDTO alterarRecursoDto);
    Task InativarAsync(int id);
}