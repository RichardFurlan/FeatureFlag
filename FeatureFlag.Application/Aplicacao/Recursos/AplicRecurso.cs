using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.Aplicacao.Recursos.DTOs;
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
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidores;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;

    public AplicRecurso(IRepRecurso repRecurso, IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor)
    {
        _repRecurso = repRecurso;
        _repConsumidores = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
    }
    #endregion
    
    #region RecuperarTodosAsync
    public async Task<List<RecuperarRecursoDto>> RecuperarTodosAsync(string query)
    {
        var recursos = await _repRecurso.RecuperarTodosAsync();
        var viewModelList = recursos.Select(r => new RecuperarRecursoDto(r.Identificacao, r.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarRecursoDto> RecuperarPorIdAsync(int id)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(id);
        var viewModel = new RecuperarRecursoDto(recurso.Identificacao, recurso.Descricao);
        return viewModel;
    }
    #endregion

    #region VerificaRecursoAtivoParaConsumidorAsync
    public async Task<RecuperarRecursoAtivoDto> VerificaRecursoAtivoParaConsumidorAsync(int recursoId, int consumidorId)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(recursoId);
        if (recurso == null)
        {
            throw new Exception($"Recurso com ID {recursoId} não encontrado.");
        }
        
        var consumidor = await _repConsumidores.RecuperarPorIdAsync(consumidorId);
        if (consumidor == null)
        {
            throw new Exception($"Consumidor com identificação {consumidorId} não encontrado.");
        }
        
        
        var recursoConsumidor = recurso.RecursoConsumidores.FirstOrDefault(rc => rc.CodigoConsumidor == consumidor.Id);
        if (recursoConsumidor == null)
        {
            throw new Exception($"Associação entre o recurso com ID {recursoId} e o consumidor com identificação {consumidorId} não encontrada.");
        }
        
        var recursoAtivoViewModel = new RecuperarRecursoAtivoDto(
            recurso.Identificacao, 
            recurso.Descricao, 
            consumidor.Identificacao, 
            recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado
        );
        
        return recursoAtivoViewModel;

    }
    #endregion

    #region InserirAsync
    public Task<int> InserirAsync(CriarRecursoDto criarRecursoDto)
    {
        var recurso = new Recurso(
            criarRecursoDto.Identificacao, 
            criarRecursoDto.Descricao,
            null,
            null
        );

        var recursoId = _repRecurso.InserirAsync(recurso);

        return recursoId;
    }
    #endregion

    #region InserirRecursoELiberacaoAsync
    public async Task<int> InserirRecursoELiberacaoAsync(CriarRecursoELiberacaoDto criarRecursoELiberacaoDto)
    {
        var recurso = new Recurso(criarRecursoELiberacaoDto.Identificacao, criarRecursoELiberacaoDto.Descricao, null, null);
        await _repRecurso.InserirAsync(recurso);

        var todosConsumidores =  await _repConsumidores.RecuperarTodosAsync();
        var totalConsumidores = todosConsumidores.Count;
        var quantidadeLiberada = (int)(totalConsumidores * criarRecursoELiberacaoDto.PercentualLiberacao / 100);

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
    
    #region AlterarPercentualDeLiberacaoDeRecurso
    public async Task AlterarPercentualDeLiberacaoDeRecurso(AlterarPercentualDeLiberacaoRecursoDto alterarPercentualRecursoDto)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(alterarPercentualRecursoDto.CodigoRecurso);
        if (recurso == null)
        {
            throw new Exception($"Recurso com ID {alterarPercentualRecursoDto.CodigoRecurso} não encontrado.");
        }

        var todosConsumidores = await _repConsumidores.RecuperarTodosAsync();
        var totalConsumidores = todosConsumidores.Count;
        var quantidadeLiberada = (int)(totalConsumidores * alterarPercentualRecursoDto.PercentualLiberacao / 100);

        var random = new Random();
        var consumidoresLiberados = todosConsumidores.OrderBy(td => random.Next()).Take(quantidadeLiberada).ToList();

        var recursoConsumidoresExistentes = recurso.RecursoConsumidores;
        foreach (var rce in recursoConsumidoresExistentes)
        {
            rce.Desabilitar();
            await _repRecursoConsumidor.AlterarAsync(rce.Id, rce);
        }
        
        foreach (var consumidor in todosConsumidores)
        {
            var status = consumidoresLiberados.Contains(consumidor)
                ? EnumStatusRecursoConsumidor.Habilitado
                : EnumStatusRecursoConsumidor.Desabilitado;

            var recursoConsumidor = recurso.RecursoConsumidores.SingleOrDefault(rc => rc.CodigoConsumidor == consumidor.Id);

            if (recursoConsumidor == null)
            {
                recursoConsumidor = new RecursoConsumidor(recurso.Id, consumidor.Id, status);
                await _repRecursoConsumidor.InserirAsync(recursoConsumidor);
            }
            else
            {
                recursoConsumidor.DefinirStatus(status);
                await _repRecursoConsumidor.AlterarAsync(recursoConsumidor.Id, recursoConsumidor);
            }
        };

    }
    #endregion
    
    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarRecursoDto alterarRecursoDto)
    {
        var recursoExistente = await _repRecurso.RecuperarPorIdAsync(id);
        var recursoAlterar = new Recurso(alterarRecursoDto.Identificacao, alterarRecursoDto.Descricao);
        
        recursoExistente.Update(recursoAlterar);
        await _repRecurso.AlterarAsync(id, recursoAlterar);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(id);
        await _repRecurso.InativarAsync(id);
        recurso.Inativar();
    }

    #endregion
}