using FeatureFlag.Application.Consumidores.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.RecursosConsumidores;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;


namespace FeatureFlag.Application.Consumidores;

public class AplicConsumidor : IAplicConsumidor
{
    #region ctor
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IRepRecurso _repRecurso;
    private readonly IAplicRecursoConsumidor _aplicRecursoConsumidor;
    public AplicConsumidor(IRepConsumidor repConsumidor, IRepRecursoConsumidor repRecursoConsumidor, IRepRecurso repRecurso, IAplicRecursoConsumidor aplicRecursoConsumidor)
    {
        _repConsumidor = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
        _repRecurso = repRecurso;
        _aplicRecursoConsumidor  = aplicRecursoConsumidor;
    }
    #endregion

    #region RecuperarTodosAsync
    public List<RecuperarConsumidorView> RecuperarTodos()
    {
        var consumidores = _repConsumidor.RecuperarTodos().ToList();
        var viewModelList = consumidores.Select(c => new RecuperarConsumidorView(c.Identificacao, c.Descricao)).ToList();
        return viewModelList;
    }
    #endregion

    #region RecuperarPorIdAsync
    public async Task<RecuperarConsumidorView> RecuperarPorIdAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        var viewModel = new RecuperarConsumidorView(consumidor.Identificacao, consumidor.Descricao);
        return viewModel;
    }
    #endregion

    #region InserirAsync
    public async Task<int> InserirAsync(CriarConsumidorDTO criarConsumidorDto)
    {
        var consumidor = new Consumidor(
            criarConsumidorDto.Identificacao, 
            criarConsumidorDto.Descricao);
        var consumidorId = await _repConsumidor.InserirAsync(consumidor);

        return consumidorId;
    }
    #endregion

    #region RecuperaRecursosPorConsumidorAsync
    public async Task<RecuperarRecursosPorConsumidorView> RecuperaRecursosPorConsumidorAsync(string identificacao)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdentificacaoAsync(identificacao);
        if (consumidor == null)
        {
            var dto = new CriarConsumidorDTO(identificacao, identificacao);
            await InserirAsync(dto);
            
            throw new Exception($"Consumidor com Identificacao {identificacao} não encontrado.");
        }

        var recursosConsumidores = await _repRecursoConsumidor.RecuperarTodosPorCodigoConsumidorAsync(consumidor.Id);
        var recursoIds = recursosConsumidores
            .Select(rc => rc.CodigoRecurso);

        var recursos = _repRecurso.RecuperarTodos().Where(r => recursoIds.Contains(r.Id)).ToList();

        var recursosStatus = recursosConsumidores
            .Join(recursos,
                rc => rc.CodigoRecurso,
                r => r.Id,
                (rc, r) => new RecuperarRecursosStatusView(r.Identificacao, rc.Status))
            .ToList();
        
        var viewModel = new RecuperarRecursosPorConsumidorView(consumidor.Identificacao, recursosStatus);
        return viewModel;
    }
    
    #endregion

    #region VerificaRecursoHabilitadoParaConsumidor
    public async Task<RecuperarRecursoAtivoConsumidorView> VerificaRecursoHabilitadoParaConsumidor(
        string identificacaoConsumidor, string identificacaoRecurso)
    {
        var dto = await _aplicRecursoConsumidor.RecuperarRecursoConsumidorAtivo(identificacaoRecurso,
            identificacaoConsumidor);
        
        var recuperarRecursoAtivoConsumidorView = new RecuperarRecursoAtivoConsumidorView(
            dto.IdentificacaoRecurso, 
            dto.DescricaoRecurso, 
            dto.IdentificacaoConsumidor, 
            dto.Habilitado
        );

        return recuperarRecursoAtivoConsumidorView;
    }
    #endregion
    
    #region AlterarAsync
    public async Task AlterarAsync(int id, AlterarConsumidorDTO alterarConsumidorDto)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        if (consumidor == null)
        {
            throw new KeyNotFoundException($"Consumidor com ID {id} não encontrado.");
        }
        
        var consumidorAlterar = new Consumidor(alterarConsumidorDto.Identificacao, alterarConsumidorDto.Descricao);
        
        consumidor.Update(consumidorAlterar);

        await _repConsumidor.AlterarAsync(consumidor);
    }
    #endregion

    #region InativarAsync
    public async Task InativarAsync(int id)
    {
        var consumidor = await _repConsumidor.RecuperarPorIdAsync(id);
        if (consumidor == null)
        {
            throw new KeyNotFoundException($"Consumidor com ID {id} não encontrado.");
        }
        consumidor.Inativar();

        await _repConsumidor.InativarAsync(consumidor);
    }
    #endregion
}