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
    public async Task<IActionResult> RecuperarTodosAsync()
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> RecuperarPorIdAsync(int id)
    {
        try
        {
            var dto = await _aplicConsumidor.RecuperarPorIdAsync(id);
            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> InserirAsync([FromBody] CriarConsumidorDto dto)
    {
        try
        {
            var id = await _aplicConsumidor.InserirAsync(dto);
            
            return CreatedAtAction(nameof(RecuperarPorIdAsync), id, dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarAsync(int id, [FromBody] AlterarConsumidorDto dto)
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
    public async Task<IActionResult> InativarAsync(int id)
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