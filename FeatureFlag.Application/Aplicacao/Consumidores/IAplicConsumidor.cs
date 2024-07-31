using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Domain.Interefaces;

public interface IAplicConsumidor
{
    Task<List<RecuperarConsumidorDTO>> RecuperarTodosAsync();
    Task<RecuperarConsumidorDTO> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(CriarConsumidorDTO criarConsumidorDto);
    Task<RecuperarRecursosPorConsumidorDTO> RecuperaRecursosPorConsumidorAsync(string identificacao);

    Task<bool> VerificaRecursoHabilitadoParaConsumidor(
        string identificacaoConsumidor, string identificacaoRecurso);
    Task AlterarAsync(int id, AlterarConsumidorDTO alterarConsumidorDto);
    Task InativarAsync(int id);
}