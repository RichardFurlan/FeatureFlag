using FeatureFlag.Application.Consumidores;
using FeatureFlag.Application.Consumidores.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Repository.Infra.CacheStorage;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ConsumidoresController : ControllerBase
{
    #region ctor
    private readonly IAplicConsumidor _aplicConsumidor;
    private readonly ICacheService _cacheService;
    public ConsumidoresController(IAplicConsumidor aplicConsumidor, ICacheService cacheService)
    {
        _aplicConsumidor = aplicConsumidor;
        _cacheService = cacheService;
    }
    #endregion

    [HttpGet]
    [OutputCache( Duration = 60)]
    public async Task<IActionResult> RecuperarTodos()
    {
        try
        {
            const string cacheKey = "Consumidores_RecuperarTodos";
            var cachedData = await _cacheService.GetAsync<List<RecuperarConsumidorView>>(cacheKey);
            if (cachedData != null)
            {
                return Ok(cachedData);
            }
            var dto = _aplicConsumidor.RecuperarTodos();
            await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

            return Ok(dto);
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger 
            return NotFound();
        }
    }
    
    //  TODO: Verificar uso do Recuperar por Id
    // [HttpGet("id/{id}")]
    // public async Task<IActionResult> RecuperarPorId(int id)
    // {
    //     try
    //     {
    //         var dto = await _aplicConsumidor.RecuperarPorIdAsync(id);
    //         return Ok(dto);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }
    
    [HttpGet("RecuperarRecursosPorIdentificacao/{identificacao}")]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> RecuperarRecursosPorIdentificacao(string identificacao)
    {
        try
        {
            var dto = await _aplicConsumidor.RecuperaRecursosPorConsumidorAsync(identificacao);
            return Ok(dto);
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger 
            return NotFound();
        }
    }
    
    [HttpGet("{identificacaoConsumidor}/recurso/{identificacaoRecurso}")]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> VerificaRecursoHabilitadoParaConsumidor(string identificacaoConsumidor, string identificacaoRecurso)
    {
        try
        {
            var dto = await _aplicConsumidor.VerificaRecursoHabilitadoParaConsumidor(identificacaoConsumidor, identificacaoRecurso);
            return Ok(dto);
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger 
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Inserir([FromBody] CriarConsumidorDTO dto)
    {
        try
        {
            var id = await _aplicConsumidor.InserirAsync(dto);
            return Created();
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger 
            return BadRequest();
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarConsumidorDTO dto)
    {
        try
        {
            await _aplicConsumidor.AlterarAsync(id, dto);
            return NoContent();
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger 
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        try
        {
            await _aplicConsumidor.InativarAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            // TODO:  Adicionar logger
            return BadRequest();
        }
    }
    
}