using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.Aplicacao;

public class AplicConsumidor : IAplicConsumidor
{
    private readonly IRepConsumidor _repConsumidorMemory;

    public AplicConsumidor(IRepConsumidor repConsumidorMemory)
    {
        _repConsumidorMemory = repConsumidorMemory;
    } 
    
    public async Task<List<ConsumidorViewModel>> ListarTodos()
    {
        var consumidores = await _repConsumidorMemory.ListarTodosAsync();
        var viewModelList = consumidores.Select(c => new ConsumidorViewModel(c.Identificacao, c.Descricao)).ToList();
        return viewModelList;
    }

    public async Task<ConsumidorViewModel> ListarPorId(int id)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        var viewModel = new ConsumidorViewModel(consumidor.Identificacao, consumidor.Descricao);
        return viewModel;
    }

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

    public async Task Alterar(int id, UpdateConsumidorInputModel updateConsumidorInputModel)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        consumidor.Update(updateConsumidorInputModel.Identificacao, updateConsumidorInputModel.Descricao);
    }

    public async Task Inativar(int id)
    {
        var consumidor = await _repConsumidorMemory.ListarPorIdAsync(id);
        consumidor.Inativar();
    }
}