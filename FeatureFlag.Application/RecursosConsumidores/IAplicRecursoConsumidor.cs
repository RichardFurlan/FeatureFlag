using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.RecursosConsumidores;

public interface IAplicRecursoConsumidor
{
    Task<int> InserirAsync(CriarRecursoConsumidorDTO criarConsumidorDto);
    Task<RecuperarRecursoConsumidorAtivoDTO> RecuperarRecursoConsumidorAtivo(string identificacaoRecurso,
        string identificacaoConsumidor);

}