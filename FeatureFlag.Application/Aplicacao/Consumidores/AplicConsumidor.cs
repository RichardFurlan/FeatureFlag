using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.Aplicacao.Consumidores;

public class AplicConsumidor : IAplicConsumidor
{
    #region ctor
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IRepRecurso _repRecurso;
    public AplicConsumidor(IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor, IRepRecurso repRecurso)
    {
        _repConsumidor = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
        _repRecurso = repRecurso;
    }
    #endregion

    #region RecuperarTodosAsync
    public async Task<List<RecuperarConsumidorDTO>> RecuperarTodosAsync()
    {
        var consumidores = await _repConsumidor.RecuperarTodosAsync();
        var viewModelList = consumidores.Select(c => new RecuperarConsumidorDTO(c.Identificacao, c.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarConsumidorDTO> RecuperarPorIdAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        var viewModel = new RecuperarConsumidorDTO(consumidor.Identificacao, consumidor.Descricao);
        return viewModel;
    }
    #endregion

    #region InserirAsync
    public async Task<int> InserirAsync(CriarConsumidorDTO criarConsumidorDto)
    {
        var consumidor = new Consumidor(
            criarConsumidorDto.Identificacao, 
            criarConsumidorDto.Descricao);
        var consumidorId = await _repConsumidor.InserirAsync(consumidor);

        return consumidorId;
    }
    #endregion

    #region RecuperaRecursosPorConsumidorAsync
    public async Task<RecuperarRecursosPorConsumidorDTO> RecuperaRecursosPorConsumidorAsync(string identificacao)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdentificacaoAsync(identificacao);
        if (consumidor == null)
        {
            throw new Exception($"Consumidor com Identificacao {identificacao} não encontrado.");
        }

        var recursosConsumidores = await _repRecursoConsumidor.RecuperarTodosPorCodigoConsumidorAsync(consumidor.Id);
        var recursosStatus = new List<RecuperarRecursosStatusDTO>();

        foreach (var rc in recursosConsumidores)
        {
            var recurso = await _repRecurso.RecuperarPorIdAsync(rc.CodigoRecurso);
            if (recurso != null)
            {
                recursosStatus.Add(new RecuperarRecursosStatusDTO(recurso.Identificacao, rc.Status));
            }
        }
        
        var viewModel = new RecuperarRecursosPorConsumidorDTO(consumidor.Identificacao, recursosStatus);
        return viewModel;
    }
    
    #endregion

    #region VerificaRecursoHabilitadoParaConsumidor
    public async Task<bool> VerificaRecursoHabilitadoParaConsumidor(
        string identificacaoConsumidor, string identificacaoRecurso)
    {
        var recurso = await _repRecurso.RecuperarPorIdentificacaoAsync(identificacaoRecurso);
        if (recurso == null)
        {
            throw new Exception($"Recurso com Identificacao {identificacaoRecurso} não encontrado.");
        }
        
        var consumidor = await _repConsumidor.RecuperarPorIdentificacaoAsync(identificacaoConsumidor);
        if (consumidor == null)
        {
            throw new Exception($"Consumidor com Identificacao {identificacaoConsumidor} não encontrado.");
        }

        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorCodigoRecursoEConsumidorAsync(recurso.Id, consumidor.Id);
        if (recursoConsumidor == null)
        {
            throw new Exception($"O recurso com Identificacao {identificacaoRecurso} não está associado ao consumidor.");
        }

        var estaLiberado = recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado;

        return estaLiberado;
    }
    #endregion
    
    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarConsumidorDTO alterarConsumidorDto)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        if (consumidor == null)
        {
            throw new KeyNotFoundException($"Consumidor com ID {id} não encontrado.");
        }
        
        var consumidorAlterar = new Consumidor(alterarConsumidorDto.Identificacao, alterarConsumidorDto.Descricao);
        
        consumidor.Update(consumidorAlterar);

        await _repConsumidor.AlterarAsync(consumidor);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        if (consumidor == null)
        {
            throw new KeyNotFoundException($"Consumidor com ID {id} não encontrado.");
        }
        consumidor.Inativar();

        await _repConsumidor.InativarAsync(consumidor);
    }
    #endregion
}