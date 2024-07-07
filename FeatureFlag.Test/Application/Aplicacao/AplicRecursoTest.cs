using FeatureFlag.Application.Aplicacao;
using FeatureFlag.Application.DTOs.InputModel;
using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;
using FeatureFlag.Domain.Repositories;
using Moq;

namespace FeatureFlag.Test.Application.Aplicacao;

public class AplicRecursoTest
{
    private readonly Mock<IRepRecurso> _repRecursoMockMemory;
    private readonly Mock<IRepConsumidor> _repConsumidorMemory;
    private readonly Mock<IRepRecursoConsumidor> _repRecursoConsumidorMemory;
    private readonly AplicRecurso _aplicRecurso;

    public AplicRecursoTest()
    {
        _repRecursoMockMemory = new Mock<IRepRecurso>();
        _repConsumidorMemory = new Mock<IRepConsumidor>();
        _repRecursoConsumidorMemory = new Mock<IRepRecursoConsumidor>();
        _aplicRecurso = new AplicRecurso(_repRecursoMockMemory.Object, _repConsumidorMemory.Object, _repRecursoConsumidorMemory.Object);
    }
    
    [Fact]
    public async Task ListarTodos_ShouldReturnAllRecursos()
    {
        // Arrange
        var recursos = new List<Recurso>
        {
            new Recurso("Rec1", "Descricao1", null, null),
            new Recurso("Rec2", "Descricao2", null, null)
        };
        _repRecursoMockMemory.Setup(r => r.ListarTodosAsync()).ReturnsAsync(recursos);

        // Act
        var result = await _aplicRecurso.ListarTodos(string.Empty);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Rec1", result[0].Identificacao);
        Assert.Equal("Rec2", result[1].Identificacao);
    }
    
    [Fact]
    public async Task ListarPorId_ShouldReturnRecurso()
    {
        // Arrange
        var recurso = new Recurso("Rec1", "Descricao1", null, null);
        _repRecursoMockMemory.Setup(r => r.ListarPorIdAsync(It.IsAny<int>())).ReturnsAsync(recurso);

        // Act
        var result = await _aplicRecurso.ListarPorId(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rec1", result.Identificacao);
        Assert.Equal("Descricao1", result.Descricao);
    }
    
    [Fact]
    public async Task VerificaRecurso_ShouldReturnRecursoAtivoViewModel()
    {
        // Arrange
        var recursoId = 1;
        var consumidorIdentificacao = 1;
        
        var recurso = new Recurso("Rec1", "Descricao1", 
            new List<Consumidor>(), 
            new List<RecursoConsumidor>());
        var consumidor = new Consumidor("Consu1", "Desc1", null, null);
        var recursoConsumidor = new RecursoConsumidor(recursoId, consumidor.Id, EnumStatusRecursoConsumidor.Habilitado);

        recurso.RecursoConsumidores.Add(recursoConsumidor);
        consumidor.RecursoConsumidores.Add(recursoConsumidor);

        _repRecursoMockMemory.Setup(r => r.ListarPorIdAsync(recursoId)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.ListarPorIdAsync(consumidorIdentificacao)).ReturnsAsync(consumidor);

        // Act
        var result = await _aplicRecurso.VerificaRecurso(recursoId, consumidorIdentificacao);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rec1", result.IdentificacaoRecurso);
        Assert.Equal("Descricao1", result.DescricaoRecurso);
        Assert.Equal("Consu1", result.IdentificacaoConsumidor);
    }
    
    [Fact]
    public async Task InserirRecursoELiberacao_ShouldCreateRecursoAndAssociations()
    {
        // Arrange
        var createRecursoELiberacaoInputModel = new CreateRecursoELiberacaoInputModel("Rec1", "Descricao1", 50);
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Cons1", "Desc1", null, null ),
            new Consumidor("Cons2", "Desc2", null, null ),
            new Consumidor("Cons3", "Desc3", null, null ),
            new Consumidor("Cons4", "Desc4", null, null ),
        };
        
        _repConsumidorMemory.Setup(c => c.ListarTodosAsync()).ReturnsAsync(consumidores);
        
        var recursoConsumidorList = new List<RecursoConsumidor>();
        _repRecursoConsumidorMemory.Setup(rc => rc.InserirAsync(It.IsAny<RecursoConsumidor>()))
            .Callback<RecursoConsumidor>(rc => recursoConsumidorList.Add(rc));

        // Act
        var result = await _aplicRecurso.InserirRecursoELiberacao(createRecursoELiberacaoInputModel);

        // Assert
        _repRecursoMockMemory.Verify(r => r.InserirAsync(It.IsAny<Recurso>()), Times.Once);
        _repRecursoConsumidorMemory.Verify(rc => rc.InserirAsync(It.IsAny<RecursoConsumidor>()), Times.Exactly(consumidores.Count));
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(createRecursoELiberacaoInputModel.PercentualLiberacao, percentualHabilitado, precision: 0);
    }

}