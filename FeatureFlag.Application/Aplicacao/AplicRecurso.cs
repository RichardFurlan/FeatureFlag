using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.Aplicacao;

public class AplicRecurso : IAplicRecurso
{
    #region ctor
    private readonly IRepRecurso _repRecursoMemory;
    private readonly IRepConsumidor _repConsumidoresMemory;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;

    public AplicRecurso(IRepRecurso repRecurso, IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor)
    {
        _repRecursoMemory = repRecurso;
        _repConsumidoresMemory = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
    }
    #endregion
    
    #region ListaTodos
    public async Task<List<RecursoViewModel>> ListarTodos(string query)
    {
        var recursos = await _repRecursoMemory.ListarTodosAsync();
        var viewModelList = recursos.Select(r => new RecursoViewModel(r.Identificacao, r.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region ListarPorId
    public async Task<RecursoViewModel> ListarPorId(int id)
    {
        var recurso = await _repRecursoMemory.ListarPorIdAsync(id);
        var viewModel = new RecursoViewModel(recurso.Identificacao, recurso.Descricao);
        return viewModel;
    }
    

    #endregion

    #region VerificaRecurso

    public async Task<RecursoAtivoViewModel> VerificaRecurso(int recursoId, int consumidorId)
    {
        var recurso = await _repRecursoMemory.ListarPorIdAsync(recursoId);
        if (recurso == null)
        {
            throw new Exception($"Recurso com ID {recursoId} não encontrado.");
        }
        
        var consumidor = await _repConsumidoresMemory.ListarPorIdAsync(consumidorId);
        if (consumidor == null)
        {
            throw new Exception($"Consumidor com identificação {consumidorId} não encontrado.");
        }
        
        
        var recursoConsumidor = recurso.RecursoConsumidores.FirstOrDefault(rc => rc.CodigoConsumidor == consumidor.Id);
        if (recursoConsumidor == null)
        {
            throw new Exception($"Associação entre o recurso com ID {recursoId} e o consumidor com identificação {consumidorId} não encontrada.");
        }
        
        var recursoAtivoViewModel = new RecursoAtivoViewModel(
            recurso.Identificacao, 
            recurso.Descricao, 
            consumidor.Identificacao, 
            recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado
        );
        
        return recursoAtivoViewModel;

    }
    #endregion

    #region Inserir
    public Task<int> Inserir(CreateRecursoInputModel createRecursoInputModel)
    {
        var recurso = new Recurso(
            createRecursoInputModel.Identificacao, 
            createRecursoInputModel.Descricao,
            null,
            null
        );

        var recursoId = _repRecursoMemory.InserirAsync(recurso);

        return recursoId;
    }
    #endregion

    #region InserirRecursoELiberacao
    public async Task<int> InserirRecursoELiberacao(CreateRecursoELiberacaoInputModel createRecursoELiberacaoInputModel)
    {
        var recurso = new Recurso(createRecursoELiberacaoInputModel.Identificacao, createRecursoELiberacaoInputModel.Descricao, null, null);
        await _repRecursoMemory.InserirAsync(recurso);

        var todosConsumidores =  await _repConsumidoresMemory.ListarTodosAsync();
        var totalConsumidores = todosConsumidores.Count;
        var quantidadeLiberada = (int)(totalConsumidores * createRecursoELiberacaoInputModel.PercentualLiberacao / 100);

        var random = new Random();
        var consumidoresLiberados = todosConsumidores.OrderBy(x => random.Next()).Take(quantidadeLiberada).ToList();
        
        todosConsumidores.ForEach( x =>
        {
            var status = consumidoresLiberados.Contains(x) ? EnumStatusRecursoConsumidor.Habilitado : EnumStatusRecursoConsumidor.Desabilitado;
            var recursoConsumidor = new RecursoConsumidor(recurso.Id, x.Id, status);
            _repRecursoConsumidor.InserirAsync(recursoConsumidor);
        });

        return recurso.Id;
    }
    #endregion
    
    #region Alterar
    public async Task Alterar(int id, UpdateRecursoInputModel updateRecursoInputModel)
    {
        var recurso = await _repRecursoMemory.ListarPorIdAsync(id);
        recurso.Update(updateRecursoInputModel.Identificacao, updateRecursoInputModel.Descricao);
    }
    #endregion

    #region Inativar

    public async Task Inativar(int id)
    {
        var recurso = await _repRecursoMemory.ListarPorIdAsync(id);
        recurso.Inativar();
    }

    #endregion
}