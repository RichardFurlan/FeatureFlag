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
    public async Task<IActionResult> Inserir([FromBody] CriarRecursoConsumidorDto criarRecursoConsumidorDto)
    {
        return CreatedAtAction(nameof(ListarPorId), new {id = 1}, criarRecursoConsumidorDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarRecursoConsumidorDto alterarRecursoConsumidorDto)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        return NoContent();
    }
}