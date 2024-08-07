using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.Consumidores;
using FeatureFlag.Application.Consumidores.DTOs;
using FeatureFlag.Application.RecursosConsumidores;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.Persistence;
using Repository.Persistence.Repositories;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicConsumidorTest
{
    private readonly Mock<IRepConsumidor> _repConsumidorMock;
    private readonly Mock<IRepRecursoConsumidor> _repRecursoConsumidorMock;
    private readonly Mock<IRepRecurso> _repRecursoMock;
    private readonly Mock<IAplicRecursoConsumidor> _aplicRecursoConsumidor;
    private readonly AplicConsumidor _aplicConsumidor;

    public AplicConsumidorTest()
    {
        _repConsumidorMock = new Mock<IRepConsumidor>();
        _repRecursoConsumidorMock = new Mock<IRepRecursoConsumidor>();
        _repRecursoMock = new Mock<IRepRecurso>();
        _aplicRecursoConsumidor = new Mock<IAplicRecursoConsumidor>();
        _aplicConsumidor = new AplicConsumidor(_repConsumidorMock.Object, _repRecursoConsumidorMock.Object, _repRecursoMock.Object, _aplicRecursoConsumidor.Object);
    }
    
    [Fact]
    public void RecuperarTodos_DeveRetornarListaConsumidorViewModel()
    {
        // Arrange
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Ident1", "Desc1"),
            new Consumidor("Ident2", "Desc2")
        }.AsQueryable();
        
        _repConsumidorMock.Setup(r => r.RecuperarTodos()).Returns(consumidores);

        
        // Act
        var result = _aplicConsumidor.RecuperarTodos();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Ident1", result[0].Identificacao);
        Assert.Equal("Desc1", result[0].Descricao);
        Assert.Equal("Ident2", result[1].Identificacao);
        Assert.Equal("Desc2", result[1].Descricao);
    }

    [Fact]
    public async Task RecuperarPorId_DeveRetornarConsumidorViewModel()
    {
        var consumidor = new Consumidor("Ident1", "Desc1");
        _repConsumidorMock.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);
        
        var result = await _aplicConsumidor.RecuperarPorIdAsync(1);
        
        // Assert
        Assert.Equal("Ident1", result.Identificacao);
        Assert.Equal("Desc1", result.Descricao);
    }
    
    [Fact]
    public async Task Inserir_NovoConsumidor()
    {
        var inputModel = new CriarConsumidorDTO("Ident1", "Desc1");
        _repConsumidorMock.Setup(r => r.InserirAsync(It.IsAny<Consumidor>())).ReturnsAsync(1);
        
        var result = await _aplicConsumidor.InserirAsync(inputModel);
        
        // Assert
        Assert.Equal(1, result);
        _repConsumidorMock.Verify(r => r.InserirAsync(It.IsAny<Consumidor>()), Times.Once);
    }
    
    [Fact]
    public async Task Alterar_ConsumidorExistente()
    {
        // Arrange
        var consumidor = new Consumidor("Ident1", "Desc1");
        var inputModel = new AlterarConsumidorDTO("IdentAlterada", "DescAlterada");

        _repConsumidorMock.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);

        // Act
        await _aplicConsumidor.AlterarAsync(consumidor.Id, inputModel);

        // Assert
        Assert.Equal("IdentAlterada", consumidor.Identificacao);
        Assert.Equal("DescAlterada", consumidor.Descricao);
        _repConsumidorMock.Verify(r => r.RecuperarPorIdAsync(It.IsAny<int>()), Times.Once);
        _repConsumidorMock.Verify(r => r.AlterarAsync(It.IsAny<Consumidor>()));
    }
    
    [Fact]
    public async Task Inativar_ConsumidorExistente()
    {
        // Arrange
        var consumidor = new Consumidor("Ident1", "Desc1");
        _repConsumidorMock.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);

        // Act
        await _aplicConsumidor.InativarAsync(1);

        // Assert
        Assert.True(consumidor.Inativo);
        _repConsumidorMock.Verify(r => r.RecuperarPorIdAsync(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task RecuperaRecursosPorConsumidor_DeveRetornarRecursosPorConsumidorViewModel()
    {
        // Arrange
        var consumidor = new Consumidor("Cons1", "Desc1");
        var recursos = new List<Recurso>()
        {
            new Recurso("Rec1", "Descricao1"),
            new Recurso("Rec2", "Descricao2")
        };
        
        typeof(BaseEntity).GetProperty("Id")?.SetValue(recursos[0], 1);
        typeof(BaseEntity).GetProperty("Id")?.SetValue(recursos[1], 2);
        
        var recursoConsumidorList = new List<RecursoConsumidor>
        {
            new RecursoConsumidor(1, consumidor.Id, EnumStatusRecursoConsumidor.Habilitado),
            new RecursoConsumidor(2, consumidor.Id, EnumStatusRecursoConsumidor.Desabilitado)
        };

        _repConsumidorMock.Setup(r => r.RecuperarPorIdentificacaoAsync(consumidor.Identificacao)).ReturnsAsync(consumidor);
        _repRecursoConsumidorMock.Setup(r => r.RecuperarTodosPorCodigoConsumidorAsync(consumidor.Id)).ReturnsAsync(recursoConsumidorList);
        _repRecursoMock.Setup(r => r.RecuperarTodos()).Returns(recursos.AsQueryable);

        // Act
        var result = await _aplicConsumidor.RecuperaRecursosPorConsumidorAsync(consumidor.Identificacao);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Cons1", result.IdentificacaoConsumidor);
        Assert.Equal(2, result.RecursosStatus.Count);
        Assert.Contains(result.RecursosStatus, r => r.IdentificacaoRecurso == "Rec1" && r.Status == EnumStatusRecursoConsumidor.Habilitado);
        Assert.Contains(result.RecursosStatus, r => r.IdentificacaoRecurso == "Rec2" && r.Status == EnumStatusRecursoConsumidor.Desabilitado);
    }
    
    [Fact]
    public async Task RecuperaRecursosPorConsumidor_QuandoNaoExisteConsumidor()
    {
        // Arrange
        var identificacaoConsumidor = "Teste";
        _repConsumidorMock.Setup(r => r.RecuperarPorIdentificacaoAsync(identificacaoConsumidor)).ReturnsAsync((Consumidor)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _aplicConsumidor.RecuperaRecursosPorConsumidorAsync(identificacaoConsumidor));
        Assert.Equal($"Consumidor com Identificacao {identificacaoConsumidor} não encontrado.", exception.Message);
    }
}

