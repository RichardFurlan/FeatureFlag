using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
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
    public async Task<IActionResult> Inserir([FromBody] CriarConsumidorDto criarConsumidorDto)
    {
        return CreatedAtAction(nameof(ListarPorId), new {id = 1}, criarConsumidorDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarConsumidorDto alterarConsumidorDto)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        return NoContent();
    }
}