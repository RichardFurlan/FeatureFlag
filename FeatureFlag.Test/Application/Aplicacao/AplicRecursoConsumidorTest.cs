using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.Consumidores;
using FeatureFlag.Application.Consumidores.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Application.Recursos;
using FeatureFlag.Application.RecursosConsumidores;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Repositories;
using Moq;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicRecursoConsumidorTest
{
    private readonly Mock<IRepRecursoConsumidor> _repRecursoConsumidorMock;
    private readonly Mock<IRepRecurso> _repRecursoMock;
    private readonly Mock<IRepConsumidor> _repConsumidorMock;
    private readonly AplicRecursoConsumidor _aplicRecursoConsumidor;

    public AplicRecursoConsumidorTest()
    {
        _repRecursoConsumidorMock = new Mock<IRepRecursoConsumidor>();
        _repRecursoMock = new Mock<IRepRecurso>();
        _repConsumidorMock = new Mock<IRepConsumidor>();
        _aplicRecursoConsumidor = new AplicRecursoConsumidor(
            _repRecursoConsumidorMock.Object,
            _repRecursoMock.Object,
            _repConsumidorMock.Object);
    }
    [Fact]
    public async Task InserirAsync_NovoRecursoConsumidor()
    {
        // Arrange
        var criarRecursoConsumidorDto = new CriarRecursoConsumidorDTO(1, 1, EnumStatusRecursoConsumidor.Habilitado);
        _repRecursoConsumidorMock.Setup(rc => rc.InserirAsync(It.IsAny<RecursoConsumidor>())).ReturnsAsync(1);

        var result = await _aplicRecursoConsumidor.InserirAsync(criarRecursoConsumidorDto);
        
        //Assert 
        Assert.Equal(1, result);
        _repRecursoConsumidorMock.Verify(rc => rc.InserirAsync(It.IsAny<RecursoConsumidor>()), Times.Once);
    }
    
}