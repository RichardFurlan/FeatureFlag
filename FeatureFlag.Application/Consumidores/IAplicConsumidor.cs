using FeatureFlag.Application.Consumidores.DTOs;

namespace FeatureFlag.Application.Consumidores;

public interface IAplicConsumidor
{
    List<RecuperarConsumidorView> RecuperarTodos();
    Task<RecuperarConsumidorView> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(CriarConsumidorDTO criarConsumidorDto);
    Task<RecuperarRecursosPorConsumidorView> RecuperaRecursosPorConsumidorAsync(string identificacao);

    Task<RecuperarRecursoAtivoConsumidorView> VerificaRecursoHabilitadoParaConsumidor(
        string identificacaoConsumidor, string identificacaoRecurso);
    Task AlterarAsync(int id, AlterarConsumidorDTO alterarConsumidorDto);
    Task InativarAsync(int id);
}