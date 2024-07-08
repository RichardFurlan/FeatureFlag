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
    private readonly IRepConsumidor _repConsumidorMemory;
    private readonly IRepRecursoConsumidor _repRecursoConsumidorMemory;
    private readonly IRepRecurso _repRecursoMemory;

    public AplicConsumidor(IRepConsumidor repConsumidorMemory, IRepRecursoConsumidor repRecursoConsumidor, IRepRecurso repRecurso)
    {
        _repConsumidorMemory = repConsumidorMemory;
        _repRecursoConsumidorMemory = repRecursoConsumidor;
        _repRecursoMemory = repRecurso;
    }
    #endregion

    #region ListarTodos
    public async Task<List<ConsumidorViewModel>> ListarTodos()
    {
        var consumidores = await _repConsumidorMemory.ListarTodosAsync();
        var viewModelList = consumidores.Select(c => new ConsumidorViewModel(c.Identificacao, c.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region ListarPorId
    public async Task<ConsumidorViewModel> ListarPorId(int id)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        var viewModel = new ConsumidorViewModel(consumidor.Identificacao, consumidor.Descricao);
        return viewModel;
    }
    #endregion

    #region Inserir
    public async Task<int> Inserir(CreateConsumidorInputModel createConsumidorInputModel)
    {
        var consumidor = new Consumidor(
            createConsumidorInputModel.Identificacao, 
            createConsumidorInputModel.Descricao,
            null,
            null
        );
        var consumidorId = await _repConsumidorMemory.InserirAsync(consumidor);

        return consumidorId;
    }

    public async Task<RecursosPorConsumidorViewModel> RecuperaRecursosPorConsumidor(int id)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        if (consumidor == null)
        {
            throw new Exception($"Consumidor com ID {id} não encontrado.");
        }

        var recursosConsumidores = await _repRecursoConsumidorMemory.ListarTodosPorConsumidor(id);
        var recursosStatus = new List<RecursosStatusViewModel>();


        foreach (var rc in recursosConsumidores)
        {
            var recurso = await _repRecursoMemory.ListarPorIdAsync(rc.CodigoRecurso);
            if (recurso != null)
            {
                recursosStatus.Add(new RecursosStatusViewModel(recurso.Identificacao, rc.Status));
            }
        }
        
        var viewModel = new RecursosPorConsumidorViewModel(consumidor.Identificacao, recursosStatus);
        return viewModel;
    }

    #endregion
    
    #region Alterar
    public async Task Alterar(int id, UpdateConsumidorInputModel updateConsumidorInputModel)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        consumidor.Update(updateConsumidorInputModel.Identificacao, updateConsumidorInputModel.Descricao);
    }
    #endregion

    #region Inativar
    public async Task Inativar(int id)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        consumidor.Inativar();
    }
    #endregion
}