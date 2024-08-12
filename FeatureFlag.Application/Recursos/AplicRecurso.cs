using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.Recursos.DTOs;
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
    private readonly IAplicRecursoConsumidor _aplicRecursoConsumidor;

    public AplicRecurso(IRepRecurso repRecurso, IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor, IAplicRecursoConsumidor aplicRecursoConsumidor)
    {
        _repRecurso = repRecurso;
        _repConsumidores = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
        _aplicRecursoConsumidor = aplicRecursoConsumidor;
    }
    #endregion
    
    #region RecuperarTodosAsync
    public List<RecuperarRecursoView> RecuperarTodos()
    {
        var recursos = _repRecurso.RecuperarTodos().ToList();
        var viewModelList = recursos.Select(r => new RecuperarRecursoView(r.Identificacao, r.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarRecursoView> RecuperarPorIdAsync(int id)
    {
        var recurso = await _repRecurso.RecuperarPorIdAsync(id);
        var viewModel = new RecuperarRecursoView(recurso.Identificacao, recurso.Descricao);
        return viewModel;
    }
    #endregion

    #region VerificaRecursoAtivoParaConsumidorIdentificacaoAsync
    public async Task<RecuperarRecursoAtivoView> VerificaRecursoHabilitado(string identificacaoRecurso, string identificacaoConsumidor)
    {
        var dto = await _aplicRecursoConsumidor.RecuperarRecursoConsumidorAtivo(identificacaoRecurso,
            identificacaoConsumidor);
        
        var recuperarRecursoAtivoView = new RecuperarRecursoAtivoView(
            dto.IdentificacaoRecurso, 
            dto.DescricaoRecurso, 
            dto.IdentificacaoConsumidor, 
            dto.Habilitado
        );

        return recuperarRecursoAtivoView;
    }
    #endregion

    #region InserirAsync
    public Task<int> InserirAsync(CriarRecursoDTO criarRecursoDto)
    {
        var recurso = new Recurso(
            criarRecursoDto.Identificacao, 
            criarRecursoDto.Descricao
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

        var todosConsumidores = _repConsumidores.RecuperarTodos();
        var totalConsumidores = todosConsumidores.Count();

        var recursoConsumidores = _repRecursoConsumidor.RecuperarTodosPorCodigoRecursoAsync(recurso.Id);
        var quantidadeHabilitados = recursoConsumidores.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        
        var quantidadeDesejada  = (int)Math.Floor(totalConsumidores * alterarPercentualRecursoDto.PercentualLiberacao / 100);

        if (quantidadeHabilitados > quantidadeDesejada)
        {
            var excedente = quantidadeHabilitados - quantidadeDesejada;
            var habilitados = recursoConsumidores
                .Where(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado)
                .OrderBy(_ => Guid.NewGuid())
                .Take(excedente)
                .ToList();
            
            foreach (var rc in habilitados)
            {
                rc.DefinirStatus(EnumStatusRecursoConsumidor.Desabilitado);
                await _repRecursoConsumidor.AlterarAsync(rc);
            }
        }
        else if (quantidadeHabilitados < quantidadeDesejada)
        {
            var diferenca = quantidadeDesejada - quantidadeHabilitados;
            var naoHabilitados = todosConsumidores
                .Where(c => !recursoConsumidores.Any(rc =>
                    rc.CodigoConsumidor == c.Id && rc.Status == EnumStatusRecursoConsumidor.Habilitado))
                .OrderBy(_ => Guid.NewGuid())
                .Take(diferenca)
                .ToList();

            foreach (var consumidor in naoHabilitados)
            {
                var recursoConsumidor = recursoConsumidores.FirstOrDefault(rc => rc.CodigoConsumidor == consumidor.Id);
                if (recursoConsumidor == null)
                {
                    var recursoConsumidorDto = new CriarRecursoConsumidorDTO(recurso.Id, consumidor.Id,
                        EnumStatusRecursoConsumidor.Habilitado);
                    await _aplicRecursoConsumidor.InserirAsync(recursoConsumidorDto);
                }
                else
                {
                    recursoConsumidor.DefinirStatus(EnumStatusRecursoConsumidor.Habilitado);
                    await _repRecursoConsumidor.AlterarAsync(recursoConsumidor);
                }
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