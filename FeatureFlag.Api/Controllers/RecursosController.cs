using FeatureFlag.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/recursos")]
public class RecursosController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> ListarPorId(int id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Inserir([FromBody] CreateRecursoInputModel createRecursoInputModel)
    {
        return CreatedAtAction(nameof(ListarPorId), new {id = 1}, createRecursoInputModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] UpdateRecursoInputModel updateRecursoInputModel)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        return NoContent();
    }
}