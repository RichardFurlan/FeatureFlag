using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
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
    public async Task<List<RecuperarRecursoConsumidorDto>> RecuperarTodosAsync()
    {
        var recursosConsumidores = await _repRecursoConsumidor.RecuperarTodosAsync();
        var viewModelList = new List<RecuperarRecursoConsumidorDto>();

        foreach (var recursoConsumidor in recursosConsumidores)
        {
            var recurso = await _aplicRecurso.RecuperarPorIdAsync(recursoConsumidor.CodigoRecurso);
            var consumidor = await _aplicConsumidor.RecuperarPorIdAsync(recursoConsumidor.CodigoConsumidor);
            
            var viewModel = new RecuperarRecursoConsumidorDto(recurso, consumidor, recursoConsumidor.Status);
            viewModelList.Add(viewModel);
        }        
        
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
    public async Task<int> InserirAsync(CriarRecursoConsumidorDto criarRecursoConsumidorDto)
    {
        var recursoConsumidor = new RecursoConsumidor(criarRecursoConsumidorDto.CodigoRecurso, criarRecursoConsumidorDto.CodigoConsumidor, criarRecursoConsumidorDto.Status);
        var codigoRecursoConsumidor = await _repRecursoConsumidor.InserirAsync(recursoConsumidor);
        return codigoRecursoConsumidor;
    }
    #endregion

    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarRecursoConsumidorDto alterarConsumidorDto)
    {
        var recursoConsumidorExistente = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        
        var recursoConsumidor = new RecursoConsumidor(alterarConsumidorDto.CodigoRecurso,
            alterarConsumidorDto.CodigoConsumidor, alterarConsumidorDto.Status);
        recursoConsumidorExistente.Update(recursoConsumidor);
        
        await _repRecursoConsumidor.AlterarAsync(id, recursoConsumidor);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        recursoConsumidor.Inativar();
        await _repRecursoConsumidor.InativarAsync(id);
    }
    #endregion

}