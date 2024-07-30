using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api")]
public class RecursosConsumidoresController : ControllerBase
{
    #region ctor

    private readonly IAplicRecursoConsumidor _aplicRecursoConsumidor;

    public RecursosConsumidoresController(IAplicRecursoConsumidor aplicRecursoConsumidor)
    {
        _aplicRecursoConsumidor = aplicRecursoConsumidor;
    }
    #endregion
    
    [HttpGet]
    public async Task<IActionResult> RecuperarTodos()
    {
        try
        {
            var dto = await _aplicRecursoConsumidor.RecuperarTodosAsync();
            return Ok(dto);
        }
        catch (Exception e)
        {
            //TODO: Adicionar logger
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> RecuperarPorId(int id)
    {
        try
        {
            var dto = await _aplicRecursoConsumidor.RecuperarPorIdAsync(id);
            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Inserir([FromBody] CriarRecursoConsumidorDto dto)
    {
        
        try
        {
            var id = await _aplicRecursoConsumidor.InserirAsync(dto);
            return CreatedAtAction(nameof(RecuperarPorId), id, dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarAsync(int id, [FromBody] AlterarRecursoConsumidorDto dto)
    {
        try
        {
            await _aplicRecursoConsumidor.AlterarAsync(id, dto);
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
            await _aplicRecursoConsumidor.InativarAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}