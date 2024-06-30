using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Repositories;
using Moq;
using Repository.Persistence.Repositories;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicConsumidorTest
{
    private readonly Mock<IRepConsumidor> _repConsumidorMock;
    private readonly AplicConsumidor _aplicConsumidor;

    public AplicConsumidorTest()
    {
        _repConsumidorMock = new Mock<IRepConsumidor>();
        _aplicConsumidor = new AplicConsumidor(_repConsumidorMock.Object);
    }
    
    [Fact]
    public async Task ListarTodos_ReturnsListOfConsumidorViewModel()
    {
        // Arrange
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Ident1", "Desc1", null, null),
            new Consumidor("Ident2", "Desc2", null, null)
        };
        _repConsumidorMock.Setup(r => r.ListarTodosAsync()).ReturnsAsync(consumidores);

        
        // Act
        var result = await _aplicConsumidor.ListarTodos();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Ident1", result[0].Identificacao);
        Assert.Equal("Desc1", result[0].Descricao);
        Assert.Equal("Ident2", result[1].Identificacao);
        Assert.Equal("Desc2", result[1].Descricao);
    }

    [Fact]
    public async Task ListarPorId_ReturnsConsumidorViewModel()
    {
        var consumidor = new Consumidor("Ident1", "Desc1", null, null);
        _repConsumidorMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);
        
        var result = await _aplicConsumidor.ListarPorId(1);
        
        // Assert
        Assert.Equal("Ident1", result.Identificacao);
        Assert.Equal("Desc1", result.Descricao);
    }
    
    [Fact]
    public async Task Inserir_AddsNewConsumidor()
    {
        var inputModel = new CreateConsumidorInputModel("Ident1", "Desc1");
        _repConsumidorMock.Setup(r => r.InserirAsync(It.IsAny<Consumidor>())).ReturnsAsync(1);
        
        var result = await _aplicConsumidor.Inserir(inputModel);
        
        // Assert
        Assert.Equal(1, result);
        _repConsumidorMock.Verify(r => r.InserirAsync(It.IsAny<Consumidor>()), Times.Once);
    }
    
    [Fact]
    public async Task Alterar_UpdatesExistingConsumidor()
    {
        // Arrange
        var consumidor = new Consumidor("Ident1", "Desc1", null, null);
        // var aplic = GenerateAplic();
        // AplicAddConsumidor(aplic, "Ident1", "Desc1");

        var inputModel = new UpdateConsumidorInputModel("IdentAlterada", "DescAlterada", null, null);

        _repConsumidorMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);

        // Act
        await _aplicConsumidor.Alterar(0, inputModel);

        // Assert
        Assert.Equal("IdentAlterada", consumidor.Identificacao);
        Assert.Equal("DescAlterada", consumidor.Descricao);
        _repConsumidorMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Inativar_InactivatesExistingConsumidor()
    {
        // Arrange
        var consumidor = new Consumidor("Ident1", "Desc1", null, null);
        _repConsumidorMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);

        // Act
        await _aplicConsumidor.Inativar(1);

        // Assert
        Assert.True(consumidor.Inativo);
        _repConsumidorMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
    }
}