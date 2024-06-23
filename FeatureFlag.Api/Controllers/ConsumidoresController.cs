using FeatureFlag.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Controllers;
[ApiController]
[Route("api/consumidores")]
public class ConsumidoresController : ControllerBase
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
    public async Task<IActionResult> Inserir([FromBody] CreateConsumidorInputModel createConsumidorInputModel)
    {
        return CreatedAtAction(nameof(ListarPorId), new {id = 1}, createConsumidorInputModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] UpdateConsumidorInputModel updateConsumidorInputModel)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        return NoContent();
    }
}