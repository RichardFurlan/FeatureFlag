using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Domain.Interefaces;

public interface IAplicConsumidor
{
    Task<List<RecuperarConsumidorDto>> RecuperarTodosAsync();
    Task<RecuperarConsumidorDto> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(CriarConsumidorDto criarConsumidorDto);
    Task<RecuperarRecursosPorConsumidorDto> RecuperaRecursosPorConsumidorAsync(int id);
    Task AlterarAsync(int id, AlterarConsumidorDto alterarConsumidorDto);
    Task InativarAsync(int id);
}