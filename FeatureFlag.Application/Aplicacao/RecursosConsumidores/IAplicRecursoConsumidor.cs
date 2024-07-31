using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Aplicacao.Interfaces;

public interface IAplicRecursoConsumidor
{
    Task<List<RecuperarRecursoConsumidorDTO>> RecuperarTodosAsync();
    Task<RecuperarRecursoConsumidorDTO> RecuperarPorIdAsync(int id);
    Task<int> InserirAsync(CriarRecursoConsumidorDTO criarConsumidorDto);
    Task AlterarAsync(int id, AlterarRecursoConsumidorDTO alterarConsumidorDto);
    Task InativarAsync(int id);
}