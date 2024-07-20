using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;
using Repository.Persistence;

namespace FeatureFlag.Application.Aplicacao;

public class AplicConsumidor : IAplicConsumidor
{
    #region ctor
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IRepRecurso _repRecurso;
    private readonly FeatureFlagDbContext _dbContext;

    public AplicConsumidor(IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor, IRepRecurso repRecurso, FeatureFlagDbContext dbContext)
    {
        _repConsumidor = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
        _repRecurso = repRecurso;
        _dbContext = dbContext;
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

        var recursosConsumidores = await _repRecursoConsumidor.RecuperarTodosPorConsumidorAsync(id);
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
        var consumidorExistente = await _repConsumidor.RecuperarPorIdAsync(id);
        
        var consumidorAlterar = new Consumidor(alterarConsumidorDto.Identificacao, alterarConsumidorDto.Descricao);
        
        consumidorExistente.Update(consumidorAlterar);

        await _dbContext.SaveChangesAsync();
        // await _repConsumidor.AlterarAsync(id, consumidorAlterar);
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