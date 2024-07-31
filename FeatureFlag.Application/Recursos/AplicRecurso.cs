using FeatureFlag.Application.Aplicacao.Recursos;
using FeatureFlag.Application.Aplicacao.Recursos.DTOs;
using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.Consumidores;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.Factory;
using FeatureFlag.Application.RecursosConsumidores;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.Recursos;

public class AplicRecurso : IAplicRecurso
{
    #region ctor
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidores;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IServiceFactory _serviceFactory;
    private IAplicConsumidor _aplicConsumidor;

    public AplicRecurso(IRepRecurso repRecurso, IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor, IServiceFactory serviceFactory, IAplicConsumidor aplicConsumidor)
    {
        _repRecurso = repRecurso;
        _repConsumidores = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
        _serviceFactory  = serviceFactory;
        _aplicConsumidor = aplicConsumidor;
    }
    
    private IAplicRecursoConsumidor AplicRecursoConsumidor => _serviceFactory.Create<IAplicRecursoConsumidor>();
    #endregion
    
    #region RecuperarTodosAsync
    public async Task<List<RecuperarRecursoDTO>> RecuperarTodosAsync()
    {
        var recursos = await _repRecurso.RecuperarTodosAsync();
        var viewModelList = recursos.Select(r => new RecuperarRecursoDTO(r.Identificacao, r.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarRecursoDTO> RecuperarPorIdAsync(int id)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(id);
        var viewModel = new RecuperarRecursoDTO(recurso.Identificacao, recurso.Descricao);
        return viewModel;
    }
    #endregion

    #region VerificaRecursoAtivoParaConsumidorIdentificacaoAsync
    public async Task<RecuperarRecursoAtivoDTO> VerificaRecursoAtivoParaConsumidorIdentificacaoAsync(string identificacaoRecurso, string identificacaoConsumidor)
    {
        var recurso = await _repRecurso.RecuperarPorIdentificacaoAsync(identificacaoRecurso);
        if (recurso == null)
        {
            throw new Exception($"Recurso com identificação {identificacaoRecurso} não encontrado.");
        }
        
        var consumidor = await _repConsumidores.RecuperarPorIdentificacaoAsync(identificacaoConsumidor);
        if (consumidor == null)
        {
            var consumidorDto = new CriarConsumidorDTO(identificacaoConsumidor, identificacaoConsumidor);
            var consumidorId = await _aplicConsumidor.InserirAsync(consumidorDto);

            consumidor = await _repConsumidores.RecuperarPorIdAsync(consumidorId);
            if (consumidor == null)
            {
                throw new Exception($"Falha ao encontrar consumidor criado com o Id: {consumidorId}");
            }
            var recursoConsumidorDto = new CriarRecursoConsumidorDTO(recurso.Id, consumidor.Id, EnumStatusRecursoConsumidor.Desabilitado);
            await AplicRecursoConsumidor.InserirAsync(recursoConsumidorDto);
        }
        
        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorCodigoRecursoEConsumidorAsync(recurso.Id, consumidor.Id);
        if (recursoConsumidor == null)
        {
            throw new Exception($"Associação entre o recurso com identificação {identificacaoRecurso} e o consumidor com identificação {identificacaoConsumidor} não encontrada.");
        }
        
        var recursoAtivoViewModel = new RecuperarRecursoAtivoDTO(
            recurso.Identificacao, 
            recurso.Descricao, 
            consumidor.Identificacao, 
            recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado
        );
        
        return recursoAtivoViewModel;
    }
    #endregion

    #region InserirAsync
    public Task<int> InserirAsync(CriarRecursoDTO criarRecursoDto)
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
            await _repRecursoConsumidor.AlterarAsync(rce);
        }
        
        foreach (var consumidor in todosConsumidores)
        {
            var status = consumidoresLiberados.Contains(consumidor)
                ? EnumStatusRecursoConsumidor.Habilitado
                : EnumStatusRecursoConsumidor.Desabilitado;

            var recursoConsumidor = recurso.RecursoConsumidores.SingleOrDefault(rc => rc.CodigoConsumidor == consumidor.Id);

            if (recursoConsumidor == null)
            {
                var recursoConsumidorDto = new CriarRecursoConsumidorDTO(recurso.Id, consumidor.Id, status);
                await AplicRecursoConsumidor.InserirAsync(recursoConsumidorDto);
            }
            else
            {
                recursoConsumidor.DefinirStatus(status);
                await _repRecursoConsumidor.AlterarAsync(recursoConsumidor);
            }
        }

    }
    #endregion
    
    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarRecursoDTO alterarRecursoDto)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(id);
        if (recurso == null)
        {
            throw new KeyNotFoundException($"Recurso com ID {id} não encontrado.");
        }
        
        var recursoAlterar = new Recurso(
            alterarRecursoDto.Identificacao, 
            alterarRecursoDto.Descricao);
        
        recurso.Update(recursoAlterar);
        await _repRecurso.AlterarAsync(recurso);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(id);
        if (recurso == null)
        {
            throw new KeyNotFoundException($"Recurso com ID {id} não encontrado.");
        }
        recurso.Inativar();
        await _repRecurso.InativarAsync(recurso);
    }

    #endregion
}