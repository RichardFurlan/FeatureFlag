using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.Aplicacao;

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
    public async Task<List<RecuperarConsumidorDto>> RecuperarTodosAsync()
    {
        var consumidores = await _repConsumidor.RecuperarTodosAsync();
        var viewModelList = consumidores.Select(c => new RecuperarConsumidorDto(c.Identificacao, c.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarConsumidorDto> RecuperarPorIdAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        var viewModel = new RecuperarConsumidorDto(consumidor.Identificacao, consumidor.Descricao);
        return viewModel;
    }
    #endregion

    #region InserirAsync
    public async Task<int> InserirAsync(CriarConsumidorDto criarConsumidorDto)
    {
        var consumidor = new Consumidor(
            criarConsumidorDto.Identificacao, 
            criarConsumidorDto.Descricao,
            null,
            null
        );
        var consumidorId = await _repConsumidor.InserirAsync(consumidor);

        return consumidorId;
    }
    #endregion

    #region RecuperaRecursosPorConsumidorAsync
    public async Task<RecuperarRecursosPorConsumidorDto> RecuperaRecursosPorConsumidorAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        if (consumidor == null)
        {
            throw new Exception($"Consumidor com ID {id} não encontrado.");
        }

        var recursosConsumidores = await _repRecursoConsumidor.RecuperarTodosPorConsumidor(id);
        var recursosStatus = new List<RecuperarRecursosStatusDto>();


        foreach (var rc in recursosConsumidores)
        {
            var recurso = await _repRecurso.RecuperarPorIdAsync(rc.CodigoRecurso);
            if (recurso != null)
            {
                recursosStatus.Add(new RecuperarRecursosStatusDto(recurso.Identificacao, rc.Status));
            }
        }
        
        var viewModel = new RecuperarRecursosPorConsumidorDto(consumidor.Identificacao, recursosStatus);
        return viewModel;
    }




    #endregion
    
    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarConsumidorDto alterarConsumidorDto)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        consumidor.Update(alterarConsumidorDto.Identificacao, alterarConsumidorDto.Descricao);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        consumidor.Inativar();
    }
    #endregion
}