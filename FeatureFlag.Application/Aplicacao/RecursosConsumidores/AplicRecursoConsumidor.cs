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
    #region ctor
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IAplicConsumidor _aplicConsumidor;
    private readonly IAplicRecurso _aplicRecurso;

    public AplicRecursoConsumidor(IRepRecursoConsumidor repRecursoConsumidor, IAplicConsumidor aplicConsumidor, IAplicRecurso aplicRecurso)
    {
        _repRecursoConsumidor = repRecursoConsumidor;
        _aplicConsumidor = aplicConsumidor;
        _aplicRecurso = aplicRecurso;
    }
    #endregion

    #region RecuperarTodosAsync
    public async Task<List<RecuperarRecursoConsumidorDto>> RecuperarTodosAsync(string query)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarTodosAsync(query);

        var recursoTasks = recursoConsumidor.Select(rc => _aplicRecurso.RecuperarPorIdAsync(rc.CodigoRecurso)).ToList();
        var consumidorTasks = recursoConsumidor.Select(rc => _aplicConsumidor.RecuperarPorIdAsync(rc.CodigoConsumidor)).ToList();

        var recursos = await Task.WhenAll(recursoTasks);
        var consumidores = await Task.WhenAll(consumidorTasks);

        var viewModelList = recursoConsumidor
            .Select((rc, index) => new RecuperarRecursoConsumidorDto(
                recursos[index],
                consumidores[index],
                rc.Status
            )).ToList();

        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarRecursoConsumidorDto> RecuperarPorIdAsync(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        var recurso = await _aplicRecurso.RecuperarPorIdAsync(recursoConsumidor.CodigoRecurso);
        var consumidor = await _aplicConsumidor.RecuperarPorIdAsync(recursoConsumidor.CodigoConsumidor);

        var viewModel = new RecuperarRecursoConsumidorDto(recurso, consumidor, recursoConsumidor.Status);
        return viewModel;
    }
    #endregion

    #region InserirAsync
    public async Task<int> InserirAsync(CriarRecursoConsumidorDto criarConsumidorDto)
    {
        var recursoConsumidor = new RecursoConsumidor(criarConsumidorDto.CodigoRecurso, criarConsumidorDto.CodigoConsumidor, criarConsumidorDto.Status);
        var codigoRecursoConsumidor = await _repRecursoConsumidor.InserirAsync(recursoConsumidor);
        return codigoRecursoConsumidor;
    }
    #endregion

    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarRecursoConsumidorDto alterarConsumidorDto)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        recursoConsumidor.Update(alterarConsumidorDto.Status);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        recursoConsumidor.Inativar();
    }
    #endregion

}