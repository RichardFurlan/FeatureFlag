using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Repositories;

namespace FeatureFlag.Application.RecursosConsumidores;

public class AplicRecursoConsumidor : IAplicRecursoConsumidor
{
    #region ctor
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidor;


    public AplicRecursoConsumidor(IRepRecursoConsumidor repRecursoConsumidor, IRepRecurso repRecurso, IRepConsumidor repConsumidor)
    {
        _repRecursoConsumidor = repRecursoConsumidor;
        _repRecurso = repRecurso;
        _repConsumidor = repConsumidor;
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
    
    #region RecuperarRecursoConsumidorAtivo
    public async Task<RecuperarRecursoConsumidorAtivoDTO> RecuperarRecursoConsumidorAtivo(string identificacaoRecurso, string identificacaoConsumidor)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdentificacaoAsync(identificacaoConsumidor);
        if (consumidor == null)
        {
            var novoConsumidor = new Consumidor(identificacaoConsumidor, identificacaoConsumidor);
            var consumidorId = await _repConsumidor.InserirAsync(novoConsumidor);
            consumidor = await _repConsumidor.RecuperarPorIdAsync(consumidorId); 
        }

        
        var recurso = await _repRecurso.RecuperarPorIdentificacaoAsync(identificacaoRecurso);
        if (recurso == null)
        {
            throw new Exception($"Recurso com identificação {identificacaoRecurso} não encontrado.");
        }

        var recursoConsumidor = await _repRecursoConsumidor.RecuperarPorCodigoRecursoEConsumidorAsync(recurso.Id, consumidor.Id);
        if (recursoConsumidor == null)
        {
            var recursoConsumidorDto = new CriarRecursoConsumidorDTO(
                recurso.Id, 
                consumidor.Id, 
                EnumStatusRecursoConsumidor.Desabilitado);
            var recursoConsumidorId = await InserirAsync(recursoConsumidorDto);
            recursoConsumidor = await _repRecursoConsumidor.RecuperarPorIdAsync(recursoConsumidorId);
        }
        
        
        var recursoAtivoViewModel = new RecuperarRecursoConsumidorAtivoDTO(
            recurso.Identificacao, 
            recurso.Descricao, 
            consumidor.Identificacao, 
            recursoConsumidor.EstaAtivo()
        );
        
        return recursoAtivoViewModel;
    }
    #endregion

}