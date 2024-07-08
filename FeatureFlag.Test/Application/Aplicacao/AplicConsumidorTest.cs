using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Repositories;
using Moq;
using Repository.Persistence.Repositories;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicConsumidorTest
{
    private readonly Mock<IRepConsumidor> _repConsumidorMock;
    private readonly Mock<IRepRecursoConsumidor> _repRecursoConsumidorMock;
    private readonly Mock<IRepRecurso> _repRecursoMock;
    private readonly AplicConsumidor _aplicConsumidor;

    public AplicConsumidorTest()
    {
        _repConsumidorMock = new Mock<IRepConsumidor>();
        _repRecursoConsumidorMock = new Mock<IRepRecursoConsumidor>();
        _repRecursoMock = new Mock<IRepRecurso>();
        _aplicConsumidor = new AplicConsumidor(_repConsumidorMock.Object, _repRecursoConsumidorMock.Object, _repRecursoMock.Object);
    }
    
    [Fact]
    public async Task ListarTodos_DeveRetornarListaConsumidorViewModel()
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
    public async Task ListarPorId_DeveRetornarConsumidorViewModel()
    {
        var consumidor = new Consumidor("Ident1", "Desc1", null, null);
        _repConsumidorMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>())).ReturnsAsync(consumidor);
        
        var result = await _aplicConsumidor.ListarPorId(1);
        
        // Assert
        Assert.Equal("Ident1", result.Identificacao);
        Assert.Equal("Desc1", result.Descricao);
    }
    
    [Fact]
    public async Task Inserir_NovoConsumidor()
    {
        var inputModel = new CreateConsumidorInputModel("Ident1", "Desc1");
        _repConsumidorMock.Setup(r => r.InserirAsync(It.IsAny<Consumidor>())).ReturnsAsync(1);
        
        var result = await _aplicConsumidor.Inserir(inputModel);
        
        // Assert
        Assert.Equal(1, result);
        _repConsumidorMock.Verify(r => r.InserirAsync(It.IsAny<Consumidor>()), Times.Once);
    }
    
    [Fact]
    public async Task Alterar_ConsumidorExistente()
    {
        // Arrange
        var consumidor = new Consumidor("Ident1", "Desc1", null, null);

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
    public async Task Inativar_ConsumidorExistente()
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
    
    [Fact]
    public async Task RecuperaRecursosPorConsumidor_DeveRetornarRecursosPorConsumidorViewModel()
    {
        // Arrange
        var consumidor = new Consumidor("Cons1", "Desc1", null, null);
        var recursoConsumidorList = new List<RecursoConsumidor>
        {
            new RecursoConsumidor(1, consumidor.Id, EnumStatusRecursoConsumidor.Habilitado),
            new RecursoConsumidor(2, consumidor.Id, EnumStatusRecursoConsumidor.Desabilitado)
        };
        var recurso1 = new Recurso("Rec1", "Descricao1", null, null);
        var recurso2 = new Recurso("Rec2", "Descricao2", null, null);

        _repConsumidorMock.Setup(r => r.ListarPorIdAsync(consumidor.Id)).ReturnsAsync(consumidor);
        _repRecursoConsumidorMock.Setup(r => r.ListarTodosPorConsumidor(consumidor.Id)).ReturnsAsync(recursoConsumidorList);
        _repRecursoMock.Setup(r => r.ListarPorIdAsync(1)).ReturnsAsync(recurso1);
        _repRecursoMock.Setup(r => r.ListarPorIdAsync(2)).ReturnsAsync(recurso2);

        // Act
        var result = await _aplicConsumidor.RecuperaRecursosPorConsumidor(consumidor.Id);

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
        var consumidorId = 1;
        _repConsumidorMock.Setup(r => r.ListarPorIdAsync(consumidorId)).ReturnsAsync((Consumidor)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _aplicConsumidor.RecuperaRecursosPorConsumidor(consumidorId));
        Assert.Equal($"Consumidor com ID {consumidorId} não encontrado.", exception.Message);
    }
}