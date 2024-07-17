using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao.Interfaces;

public interface IAplicRecursoConsumidor
{
    Task<List<RecuperarRecursoConsumidorDto>> RecuperarTodosAsync();
    Task<RecuperarRecursoConsumidorDto> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(CriarRecursoConsumidorDto criarConsumidorDto);
    Task AlterarAsync(int id, AlterarRecursoConsumidorDto alterarConsumidorDto);
    Task InativarAsync(int id);
}