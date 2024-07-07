using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.Aplicacao;

public class AplicRecursoConsumidor : IAplicRecursoConsumidor
{
    private readonly IRepRecursoConsumidor _repRecursoConsumidorMemory;
    private readonly IAplicConsumidor _aplicConsumidor;
    private readonly IAplicRecurso _aplicRecurso;

    public AplicRecursoConsumidor(IRepRecursoConsumidor repRecursoConsumidor, IAplicConsumidor aplicConsumidor, IAplicRecurso aplicRecurso)
    {
        _repRecursoConsumidorMemory = repRecursoConsumidor;
        _aplicConsumidor = aplicConsumidor;
        _aplicRecurso = aplicRecurso;
    }
    public async Task<List<RecursoConsumidorViewModel>> ListarTodos(string query)
    {
        var recursoConsumidor = await _repRecursoConsumidorMemory.ListarTodosAsync(query);

        var recursoTasks = recursoConsumidor.Select(rc => _aplicRecurso.ListarPorId(rc.CodigoRecurso)).ToList();
        var consumidorTasks = recursoConsumidor.Select(rc => _aplicConsumidor.ListarPorId(rc.CodigoConsumidor)).ToList();

        var recursos = await Task.WhenAll(recursoTasks);
        var consumidores = await Task.WhenAll(consumidorTasks);

        var viewModelList = recursoConsumidor
            .Select((rc, index) => new RecursoConsumidorViewModel(
                recursos[index],
                consumidores[index],
                rc.Status
            )).ToList();

        return viewModelList;
    }

    public async Task<RecursoConsumidorViewModel> ListarPorId(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidorMemory.ListarPorIdAsync(id);
        var recurso = await _aplicRecurso.ListarPorId(recursoConsumidor.CodigoRecurso);
        var consumidor = await _aplicConsumidor.ListarPorId(recursoConsumidor.CodigoConsumidor);

        var viewModel = new RecursoConsumidorViewModel(recurso, consumidor, recursoConsumidor.Status);
        return viewModel;
    }

    public async Task<int> Inserir(CreateRecursoConsumidorInputModel createConsumidorInputModel)
    {
        var recursoConsumidor = new RecursoConsumidor(createConsumidorInputModel.CodigoRecurso, createConsumidorInputModel.CodigoConsumidor, createConsumidorInputModel.Status);
        var codigoRecursoConsumidor = await _repRecursoConsumidorMemory.InserirAsync(recursoConsumidor);
        return codigoRecursoConsumidor;
    }

    public async Task Alterar(int id, UpdateRecursoConsumidorInputModel updateConsumidorInputModel)
    {
        var recursoConsumidor = await _repRecursoConsumidorMemory.ListarPorIdAsync(id);
        recursoConsumidor.Update(updateConsumidorInputModel.Status);
    }

    public async Task Inativar(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidorMemory.ListarPorIdAsync(id);
        recursoConsumidor.Inativar();
    }
}