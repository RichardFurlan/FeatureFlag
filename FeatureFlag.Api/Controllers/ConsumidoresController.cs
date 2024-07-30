using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Domain.Interefaces;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/consumidores")]
public class ConsumidoresController : ControllerBase
{
    #region ctor
    private readonly IAplicConsumidor _aplicConsumidor;
    public ConsumidoresController(IAplicConsumidor aplicConsumidor)
    {
        _aplicConsumidor = aplicConsumidor;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> RecuperarTodos()
    {
        try
        {
            var dto = await _aplicConsumidor.RecuperarTodosAsync();
            return Ok(dto);
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger 
            Console.WriteLine(e);
            throw;
        }
    }
    
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
    public async Task<IActionResult> RecuperarRecursosPorIdentificacao(string identificacao)
    {
        try
        {
            var dto = await _aplicConsumidor.RecuperaRecursosPorConsumidorAsync(identificacao);
            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    [HttpGet("{identificacaoConsumidor}/recurso/{identificacaoRecurso}")]
    public async Task<IActionResult> VerificaRecursoHabilitadoParaConsumidor(string identificacaoConsumidor, string identificacaoRecurso)
    {
        try
        {
            var dto = await _aplicConsumidor.VerificaRecursoHabilitadoParaConsumidor(identificacaoConsumidor, identificacaoRecurso);
            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Inserir([FromBody] CriarConsumidorDto dto)
    {
        try
        {
            var id = await _aplicConsumidor.InserirAsync(dto);
            
            return Created();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarConsumidorDto dto)
    {
        try
        {
            await _aplicConsumidor.AlterarAsync(id, dto);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
            Console.WriteLine(e);
            throw;
        }
    }
    
}