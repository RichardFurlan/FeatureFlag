using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.Aplicacao.Recursos.DTOs;
using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
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
            Console.WriteLine(e);
            throw;
        }
    }
    
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
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("inserir")]
    public async Task<IActionResult> Inserir([FromBody] CriarRecursoDto dto)
    {

        try
        {
            var id = await _aplicRecurso.InserirAsync(dto);
            return Created();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost("inserir-liberar")]
    public async Task<IActionResult> InserirELiberar([FromBody] CriarRecursoELiberacaoDto dto)
    {

        try
        {
            var id = await _aplicRecurso.InserirRecursoELiberacaoAsync(dto);
            return Created();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarRecursoDto dto)
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
    
    [HttpPut("percentual/{id}")]
    public async Task<IActionResult> AlterarPercentual([FromBody] AlterarPercentualDeLiberacaoRecursoDto dto)
    {
        
        try
        {
            await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(dto);
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