using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Interefaces;
using FeatureFlag.Domain.Repositories;
using Moq;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicRecursoConsumidorTest
{
    private readonly Mock<IRepRecursoConsumidor> _repRecursoConsumidorMock;
    private readonly Mock<IAplicRecurso> _aplicRecursoMock;
    private readonly Mock<IAplicConsumidor> _aplicConsumidorMock;
    private readonly AplicRecursoConsumidor _aplicRecursoConsumidor;

    public AplicRecursoConsumidorTest()
    {
        _repRecursoConsumidorMock = new Mock<IRepRecursoConsumidor>();
        _aplicConsumidorMock = new Mock<IAplicConsumidor>();
        _aplicRecursoMock = new Mock<IAplicRecurso>();
        _aplicRecursoConsumidor = new AplicRecursoConsumidor(
            _repRecursoConsumidorMock.Object,
            _aplicConsumidorMock.Object,
            _aplicRecursoMock.Object);
    }
    
    [Fact]
    public async Task RecuperarTodos_DeveRetornarListaRecursoConsumidorViewModel()
    {
        // Arrange
        var consumidores = new List<RecuperarConsumidorDto>
        {
            new RecuperarConsumidorDto("Ident1", "Desc1"),
            new RecuperarConsumidorDto("Ident2", "Desc2")
        };
        
        var recursos = new List<RecuperarRecursoDto>
        {
            new RecuperarRecursoDto("Ident1", "Desc1"),
            new RecuperarRecursoDto("Ident2", "Desc2")
        };

        var recursosConsumidores = new List<RecursoConsumidor>
        {
            new RecursoConsumidor(0, 0, EnumStatusRecursoConsumidor.Habilitado),
            new RecursoConsumidor(1, 1, EnumStatusRecursoConsumidor.Desabilitado)
        };
        
        _repRecursoConsumidorMock.Setup(r => r.RecuperarTodosAsync()).ReturnsAsync(recursosConsumidores);
        _aplicRecursoMock.Setup(r => r.RecuperarPorIdAsync(It.Is<int>(id => id == 0))).ReturnsAsync(recursos[0]);
        _aplicRecursoMock.Setup(r => r.RecuperarPorIdAsync(It.Is<int>(id => id == 1))).ReturnsAsync(recursos[1]);
        _aplicConsumidorMock.Setup(c => c.RecuperarPorIdAsync(It.Is<int>(id => id == 0))).ReturnsAsync(consumidores[0]);
        _aplicConsumidorMock.Setup(c => c.RecuperarPorIdAsync(It.Is<int>(id => id == 1))).ReturnsAsync(consumidores[1]);

        // Act
        var result = await _aplicRecursoConsumidor.RecuperarTodosAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Ident1", result[0].RecuperarRecursoDto.Identificacao);
        Assert.Equal("Ident1", result[0].RecuperarConsumidor.Identificacao);
        Assert.Equal(EnumStatusRecursoConsumidor.Habilitado, result[0].Status);
        Assert.Equal("Ident2", result[1].RecuperarRecursoDto.Identificacao);
        Assert.Equal("Ident2", result[1].RecuperarConsumidor.Identificacao);
        Assert.Equal(EnumStatusRecursoConsumidor.Desabilitado, result[1].Status);
        
        _repRecursoConsumidorMock.Verify(r => r.RecuperarTodosAsync(), Times.Once);
        _aplicRecursoMock.Verify(r => r.RecuperarPorIdAsync(It.IsAny<int>()), Times.Exactly(2));
        _aplicConsumidorMock.Verify(c => c.RecuperarPorIdAsync(It.IsAny<int>()), Times.Exactly(2));
    }
    
        [Fact]
    public async Task RecuperarPorId_DeveRetornarRecursoConsumidorViewModel()
    {
        // Arrange
        var consumidorDto = new RecuperarConsumidorDto("Ident1", "Desc1");
        var recursoDto = new RecuperarRecursoDto("Ident1", "Desc1");

        var recursoConsumidor = new RecursoConsumidor(0, 0, EnumStatusRecursoConsumidor.Habilitado);
            
        
        _repRecursoConsumidorMock.Setup(r => r.RecuperarPorIdAsync(It.IsAny<int>())).ReturnsAsync(recursoConsumidor);
        _aplicRecursoMock.Setup(r => r.RecuperarPorIdAsync(It.Is<int>(id => id == 0))).ReturnsAsync(recursoDto);
        _aplicConsumidorMock.Setup(c => c.RecuperarPorIdAsync(It.Is<int>(id => id == 0))).ReturnsAsync(consumidorDto);


        // Act
        var result = await _aplicRecursoConsumidor.RecuperarPorIdAsync(recursoConsumidor.Id);

        // Assert
        Assert.Equal("Ident1", result.RecuperarRecursoDto.Identificacao);
        Assert.Equal("Ident1", result.RecuperarConsumidor.Identificacao);
        Assert.Equal("Desc1", result.RecuperarRecursoDto.Descricao);
        Assert.Equal("Desc1", result.RecuperarConsumidor.Descricao);
        Assert.Equal(EnumStatusRecursoConsumidor.Habilitado, result.Status);
        
        _repRecursoConsumidorMock.Verify(r => r.RecuperarPorIdAsync(It.IsAny<int>()), Times.Once);
        _aplicRecursoMock.Verify(r => r.RecuperarPorIdAsync(It.IsAny<int>()), Times.Once);
        _aplicConsumidorMock.Verify(c => c.RecuperarPorIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task InserirAsync_NovoRecursoConsumidor()
    {
        // Arrange
        var criarRecursoConsumidorDto = new CriarRecursoConsumidorDto(1, 1, EnumStatusRecursoConsumidor.Habilitado);
        _repRecursoConsumidorMock.Setup(rc => rc.InserirAsync(It.IsAny<RecursoConsumidor>())).ReturnsAsync(1);

        var result = await _aplicRecursoConsumidor.InserirAsync(criarRecursoConsumidorDto);
        
        //Assert 
        Assert.Equal(1, result);
        _repRecursoConsumidorMock.Verify(rc => rc.InserirAsync(It.IsAny<RecursoConsumidor>()), Times.Once);
    }
    
}