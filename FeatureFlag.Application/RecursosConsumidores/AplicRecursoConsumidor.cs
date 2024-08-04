using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.Consumidores;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.Recursos;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlag.Application.RecursosConsumidores;

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
    public async Task<List<RecuperarRecursoConsumidorDTO>> RecuperarTodos()
    {
        var recursosConsumidores = _repRecursoConsumidor.RecuperarTodos().ToList();
        var viewModelList = new List<RecuperarRecursoConsumidorDTO>();

        foreach (var recursoConsumidor in recursosConsumidores)
        {
            var recurso = await _aplicRecurso.RecuperarPorIdAsync(recursoConsumidor.CodigoRecurso);
            var consumidor = await _aplicConsumidor.RecuperarPorIdAsync(recursoConsumidor.CodigoConsumidor);
            
            var viewModel = new RecuperarRecursoConsumidorDTO(recurso, consumidor, recursoConsumidor.Status);
            viewModelList.Add(viewModel);
        }        
        
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarRecursoConsumidorDTO> RecuperarPorIdAsync(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        if (recursoConsumidor is null)
        {
            throw new KeyNotFoundException($"RecursoConsumidor com ID {id} não encontrado.");
        }
        var recurso = await _aplicRecurso.RecuperarPorIdAsync(recursoConsumidor.CodigoRecurso);
        var consumidor = await _aplicConsumidor.RecuperarPorIdAsync(recursoConsumidor.CodigoConsumidor);

        var viewModel = new RecuperarRecursoConsumidorDTO(recurso, consumidor, recursoConsumidor.Status);
        return viewModel;
    }
    #endregion

    #region InserirAsync
    public async Task<int> InserirAsync(CriarRecursoConsumidorDTO dto)
    {
        var recursoConsumidor = new RecursoConsumidor(dto.CodigoRecurso, dto.CodigoConsumidor, dto.Status);
        var codigoRecursoConsumidor = await _repRecursoConsumidor.InserirAsync(recursoConsumidor);
        return codigoRecursoConsumidor;
    }
    #endregion

    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarRecursoConsumidorDTO dto)
    {
        var recursoConsumidor= await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        if (recursoConsumidor == null)
        {
            throw new KeyNotFoundException($"RecursoConsumidor com ID {id} não encontrado.");
        }
        
        var recursoConsumidorAlterar = new RecursoConsumidor(dto.CodigoRecurso,
            dto.CodigoConsumidor, dto.Status);
        
        recursoConsumidor.Update(recursoConsumidorAlterar);
        await _repRecursoConsumidor.AlterarAsync(recursoConsumidor);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(id);
        if (recursoConsumidor == null)
        {
            throw new KeyNotFoundException($"RecursoConsumidor com ID {id} não encontrado.");
        }
        recursoConsumidor.Inativar();
        await _repRecursoConsumidor.InativarAsync(recursoConsumidor);
    }
    #endregion

}