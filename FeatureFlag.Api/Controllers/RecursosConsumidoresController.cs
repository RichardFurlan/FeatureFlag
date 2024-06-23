using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/recurso/{recursoId}/consumidor/{consumidorId}")]
public class RecursosConsumidoresController : ControllerBase
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
    public async Task<IActionResult> Inserir([FromBody] CreateRecursoConsumidorInputModel createRecursoConsumidorInputModel)
    {
        return CreatedAtAction(nameof(ListarPorId), new {id = 1}, createRecursoConsumidorInputModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] UpdateRecursoConsumidorInputModel updateRecursoConsumidorInputModel)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        return NoContent();
    }
}