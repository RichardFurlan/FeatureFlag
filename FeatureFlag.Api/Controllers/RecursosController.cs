using FeatureFlag.Application.Recursos;
using FeatureFlag.Application.Recursos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> RecuperarTodos()
    {
        try
        {
            var dto = await _aplicRecurso.RecuperarTodosAsync();
            return Ok(dto);
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            return NotFound();
        }
    }
    
    //  TODO: Verificar uso do Recuperar por Id
    // [HttpGet("{id}")]
    // public async Task<IActionResult> RecuperarPorId(int id)
    // {
    //     try
    //     {
    //         var dto = await _aplicRecurso.RecuperarPorIdAsync(id);
    //         return Ok(dto);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }

    [HttpGet("{identificacaoRecurso}/consumidor/{identificacaoConsumidor}")]
    public async Task<IActionResult> RecuperaRecurso(string identificacaoRecurso, string identificacaoConsumidor)
    {
        try
        {
            var dto = await _aplicRecurso.VerificaRecursoAtivoParaConsumidorIdentificacaoAsync(identificacaoRecurso, identificacaoConsumidor);
            return Ok(dto);
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            return NotFound();
        }
    }

    [HttpPost("inserir")]
    public async Task<IActionResult> Inserir([FromBody] CriarRecursoDTO dto)
    {

        try
        {
            var id = await _aplicRecurso.InserirAsync(dto);
            return Created();
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarRecursoDTO dto)
    {
        
        try
        {
            await _aplicRecurso.AlterarAsync(id, dto);
            return NoContent();
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            return BadRequest();
        }
    }
    
    [HttpPut("{id}/alterarPercentual")]
    public async Task<IActionResult> AlterarPercentual([FromBody] AlterarPercentualDeLiberacaoRecursoDto dto)
    {
        
        try
        {
            await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(dto);
            return NoContent();
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        
        try
        {
            await _aplicRecurso.InativarAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            // TODO: Adicionar logger
            return BadRequest();
        }
    }
}