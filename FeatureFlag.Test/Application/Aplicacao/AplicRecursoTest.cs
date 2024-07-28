using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.Aplicacao.Interfaces;
using FeatureFlag.Application.Aplicacao.Recursos;
using FeatureFlag.Application.Aplicacao.Recursos.DTOs;
using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;
using Moq;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicRecursoTest
{
    private readonly Mock<IRepRecurso> _repRecursoMockMemory;
    private readonly Mock<IRepConsumidor> _repConsumidorMemory;
    private readonly Mock<IRepRecursoConsumidor> _repRecursoConsumidorMemory;
    private readonly Mock<IAplicRecursoConsumidor> _aplicRecursoConsumidor;
    private readonly Mock<IAplicConsumidor> _aplicConsumidor;
    private readonly AplicRecurso _aplicRecurso;

    public AplicRecursoTest()
    {
        _repRecursoMockMemory = new Mock<IRepRecurso>();
        _repConsumidorMemory = new Mock<IRepConsumidor>();
        _repRecursoConsumidorMemory = new Mock<IRepRecursoConsumidor>();
        _aplicRecursoConsumidor = new Mock<IAplicRecursoConsumidor>();
        _aplicConsumidor = new Mock<IAplicConsumidor>();
        _aplicRecurso = new AplicRecurso(_repRecursoMockMemory.Object, _repConsumidorMemory.Object, _repRecursoConsumidorMemory.Object, _aplicRecursoConsumidor.Object, _aplicConsumidor.Object);
    }
    
    [Fact]
    public async Task RecuperarTodos_DeveRetornarRecuperarecursoViewModel()
    {
        // Arrange
        var recursos = new List<Recurso>
        {
            new Recurso("Rec1", "Descricao1", null, null),
            new Recurso("Rec2", "Descricao2", null, null)
        };
        _repRecursoMockMemory.Setup(r => r.RecuperarTodosAsync()).ReturnsAsync(recursos);

        // Act
        var result = await _aplicRecurso.RecuperarTodosAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Rec1", result[0].Identificacao);
        Assert.Equal("Rec2", result[1].Identificacao);
    }
    
    [Fact]
    public async Task RecuperarPorId_DeveRetornarRecursoViewModel()
    {
        // Arrange
        var recurso = new Recurso("Rec1", "Descricao1", null, null);
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(recurso);

        // Act
        var result = await _aplicRecurso.RecuperarPorIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rec1", result.Identificacao);
        Assert.Equal("Descricao1", result.Descricao);
    }
    
    [Fact]
    public async Task VerificaRecurso_DeveRetornarRecursoAtivoViewModel()
    {
        // Arrange
        
        var recurso = new Recurso("Recurso1", "Descricao1");
        var consumidor = new Consumidor("Consumidor1", "Descricao1", null, null);
        var recursoConsumidor = new RecursoConsumidor(recurso.Id, consumidor.Id, EnumStatusRecursoConsumidor.Habilitado);

        recurso.RecursoConsumidores.Add(recursoConsumidor);
        consumidor.RecursoConsumidores.Add(recursoConsumidor);

        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdentificacaoAsync(recurso.Identificacao)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.RecuperarPorIdentificacaoAsync(consumidor.Identificacao)).ReturnsAsync(consumidor);

        // Act
        var result = await _aplicRecurso.VerificaRecursoAtivoParaConsumidorIdentificacaoAsync(recurso.Identificacao, consumidor.Identificacao);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Recurso1", result.IdentificacaoRecurso);
        Assert.Equal("Descricao1", result.DescricaoRecurso);
        Assert.Equal("Consumidor1", result.IdentificacaoConsumidor);
    }
    
    [Fact]
    public async Task InserirRecursoELiberacao_DeveCriarRecursoELiberarConformePercentual50()
    {
        // Arrange
        var createRecursoELiberacaoInputModel = new CriarRecursoELiberacaoDto("Rec1", "Descricao1", 50);
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Cons1", "Desc1", null, null ),
            new Consumidor("Cons2", "Desc2", null, null ),
            new Consumidor("Cons3", "Desc3", null, null ),
            new Consumidor("Cons4", "Desc4", null, null ),
        };
        
        _repConsumidorMemory.Setup(c => c.RecuperarTodosAsync()).ReturnsAsync(consumidores);
        
        var recursoConsumidorList = new List<CriarRecursoConsumidorDto>();
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()))
            .Callback<CriarRecursoConsumidorDto>(rc => recursoConsumidorList.Add(rc));

        // Act
        var result = await _aplicRecurso.InserirRecursoELiberacaoAsync(createRecursoELiberacaoInputModel);

        // Assert
        _repRecursoMockMemory.Verify(r => r.InserirAsync(It.IsAny<Recurso>()), Times.Once);
        _aplicRecursoConsumidor.Verify(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()), Times.Exactly(consumidores.Count));
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(createRecursoELiberacaoInputModel.PercentualLiberacao, percentualHabilitado, precision: 0);
    }
    
    [Fact]
    public async Task InserirRecursoELiberacao_DeveCriarRecursoELiberarConformePercentual75()
    {
        // Arrange
        var createRecursoELiberacaoInputModel = new CriarRecursoELiberacaoDto("Rec1", "Descricao1", 75);
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Cons1", "Desc1", null, null ),
            new Consumidor("Cons2", "Desc2", null, null ),
            new Consumidor("Cons3", "Desc3", null, null ),
            new Consumidor("Cons4", "Desc4", null, null ),
        };
        
        _repConsumidorMemory.Setup(c => c.RecuperarTodosAsync()).ReturnsAsync(consumidores);
        
        var recursoConsumidorList = new List<CriarRecursoConsumidorDto>();
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()))
            .Callback<CriarRecursoConsumidorDto>(rc => recursoConsumidorList.Add(rc));

        // Act
        var result = await _aplicRecurso.InserirRecursoELiberacaoAsync(createRecursoELiberacaoInputModel);

        // Assert
        _repRecursoMockMemory.Verify(r => r.InserirAsync(It.IsAny<Recurso>()), Times.Once);
        _aplicRecursoConsumidor.Verify(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()), Times.Exactly(consumidores.Count));
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(createRecursoELiberacaoInputModel.PercentualLiberacao, percentualHabilitado, precision: 0);
    }
    
   [Fact]
    public async Task AlterarPercentualDeLiberacaoDeRecurso_DeveAlterarPercentualCorretamente50()
    {
        // Arrange
        var recursoId = 1;
        var percentualLiberacao = 50.0m;

        var alterarPercentualRecursoDto = new AlterarPercentualDeLiberacaoRecursoDto(recursoId, percentualLiberacao);

        var consumidores = new List<Consumidor>
        {
            new Consumidor("Cons1", "Desc1", null, null),
            new Consumidor("Cons2", "Desc2", null, null),
            new Consumidor("Cons3", "Desc3", null, null),
            new Consumidor("Cons4", "Desc4", null, null)
        };
        
        
        var recurso = new Recurso("Recurso1", "Descricao1", consumidores, null);
        
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(recursoId)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.RecuperarTodosAsync()).ReturnsAsync(consumidores);
        _repRecursoConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>())).Returns(Task.CompletedTask);
        _repRecursoMockMemory.Setup(rc => rc.AlterarAsync(It.IsAny<Recurso>())).Returns(Task.CompletedTask);
        _repConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<Consumidor>())).Returns(Task.CompletedTask);
        var recursoConsumidorList = new List<CriarRecursoConsumidorDto>();
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()))
            .Callback<CriarRecursoConsumidorDto>(rc => recursoConsumidorList.Add(rc));

        // Act
        await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(alterarPercentualRecursoDto);

        // Assert
        _repRecursoMockMemory.Verify(r => r.RecuperarPorIdAsync(recursoId), Times.Once);
        _repConsumidorMemory.Verify(c => c.RecuperarTodosAsync(), Times.Once);
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(percentualLiberacao, percentualHabilitado, precision: 0);

        _repRecursoConsumidorMemory.Verify(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>()), Times.Exactly(recurso.RecursoConsumidores.Count));
        _aplicRecursoConsumidor.Verify(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()), Times.Exactly(consumidores.Count - recurso.RecursoConsumidores.Count));
    }
    
    [Fact]
    public async Task AlterarPercentualDeLiberacaoDeRecurso_DeveAlterarPercentualCorretamente75()
    {
        // Arrange
        var recursoId = 1;
        var percentualLiberacao = 75.0m;

        var alterarPercentualRecursoDto = new AlterarPercentualDeLiberacaoRecursoDto(recursoId, percentualLiberacao);

        var consumidores = new List<Consumidor>
        {
            new Consumidor("Cons1", "Desc1", null, null),
            new Consumidor("Cons2", "Desc2", null, null),
            new Consumidor("Cons3", "Desc3", null, null),
            new Consumidor("Cons4", "Desc4", null, null)
        };
        
        
        var recurso = new Recurso("Recurso1", "Descricao1", consumidores, null);
        
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(recursoId)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.RecuperarTodosAsync()).ReturnsAsync(consumidores);
        _repRecursoConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>())).Returns(Task.CompletedTask);
        _repRecursoMockMemory.Setup(rc => rc.AlterarAsync(It.IsAny<Recurso>())).Returns(Task.CompletedTask);
        _repConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<Consumidor>())).Returns(Task.CompletedTask);
        var recursoConsumidorList = new List<CriarRecursoConsumidorDto>();
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()))
            .Callback<CriarRecursoConsumidorDto>(rc => recursoConsumidorList.Add(rc));

        // Act
        await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(alterarPercentualRecursoDto);

        // Assert
        _repRecursoMockMemory.Verify(r => r.RecuperarPorIdAsync(recursoId), Times.Once);
        _repConsumidorMemory.Verify(c => c.RecuperarTodosAsync(), Times.Once);
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(percentualLiberacao, percentualHabilitado, precision: 0);

        _repRecursoConsumidorMemory.Verify(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>()), Times.Exactly(recurso.RecursoConsumidores.Count));
        _aplicRecursoConsumidor.Verify(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDto>()), Times.Exactly(consumidores.Count - recurso.RecursoConsumidores.Count));
    }

    [Fact]
    public async Task Alterar_RecursoExistente()
    {
        // Arrange
        var recurso = new Recurso("Ident1", "Desc1");

        var inputModel = new AlterarRecursoDto("IdentAlterada", "DescAlterada");
    
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(recurso);
        _repRecursoMockMemory.Setup(r => r.AlterarAsync(It.IsAny<Recurso>())).Returns(Task.CompletedTask);


        // Act
        await _aplicRecurso.AlterarAsync(recurso.Id, inputModel);

        // Assert
        Assert.Equal("IdentAlterada", recurso.Identificacao);
        Assert.Equal("DescAlterada", recurso.Descricao);
        _repRecursoMockMemory.Verify(r => r.AlterarAsync(It.IsAny<Recurso>()), Times.Once);
    }
    
    [Fact]
    public async Task Inativar_RecursoExistente()
    {
        // Arrange
        var recurso = new Recurso("Ident1", "Desc1");
        
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(recurso);
        _repRecursoMockMemory.Setup(r => r.InativarAsync(It.IsAny<Recurso>())).Returns(Task.CompletedTask);


        // Act
        await _aplicRecurso.InativarAsync(recurso.Id);

        // Assert
        Assert.True(recurso.Inativo);
        _repRecursoMockMemory.Verify(r => r.InativarAsync(It.IsAny<Recurso>()), Times.Once);
    }


}