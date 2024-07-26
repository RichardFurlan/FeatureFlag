using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/recursos")]
public class RecursosController : ControllerBase
{
    #region ctor

    private readonly IAplicRecurso _aplicRecurso;
    public RecursosController(IAplicRecurso aplicRecurso)
    {
        _aplicRecurso = aplicRecurso;
    }
    

    #endregion
    
    [HttpGet]
    public async Task<IActionResult> RecuperarTodosAsync()
    {
        try
        {
            var dto = await _aplicRecurso.RecuperarTodosAsync();
            return Ok(dto);
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> RecuperarPorIdAsync(int id)
    {

        try
        {
            var dto = await _aplicRecurso.RecuperarPorIdAsync(id);
            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> InserirAsync([FromBody] CriarRecursoDto dto)
    {

        try
        {
            var id = await _aplicRecurso.InserirAsync(dto);
            return CreatedAtAction(nameof(RecuperarPorIdAsync), id, dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarAsync(int id, [FromBody] AlterarRecursoDto dto)
    {
        
        try
        {
            await _aplicRecurso.AlterarAsync(id, dto);
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
            await _aplicRecurso.InativarAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}