using FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;
using FeatureFlag.Application.Recursos;
using FeatureFlag.Application.Recursos.DTOs;
using FeatureFlag.Application.RecursosConsumidores;
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
    private readonly Mock<IAplicRecursoConsumidor> _aplicRecursoConsumidor;
    private readonly AplicRecurso _aplicRecurso;

    public AplicRecursoTest()
    {
        _repRecursoMockMemory = new Mock<IRepRecurso>();
        _repConsumidorMemory = new Mock<IRepConsumidor>();
        _repRecursoConsumidorMemory = new Mock<IRepRecursoConsumidor>();
        _aplicRecursoConsumidor = new Mock<IAplicRecursoConsumidor>();
        _aplicRecurso = new AplicRecurso(_repRecursoMockMemory.Object, _repConsumidorMemory.Object, _repRecursoConsumidorMemory.Object, _aplicRecursoConsumidor.Object);
    }
    
    [Fact]
    public void RecuperarTodos_DeveRetornarRecuperarecursoViewModel()
    {
        // Arrange
        var recursos = new List<Recurso>
        {
            new Recurso("Rec1", "Descricao1"),
            new Recurso("Rec2", "Descricao2")
        };
        _repRecursoMockMemory.Setup(r => r.RecuperarTodos()).Returns(recursos.AsQueryable);

        // Act
        var result = _aplicRecurso.RecuperarTodos();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Rec1", result[0].Identificacao);
        Assert.Equal("Rec2", result[1].Identificacao);
    }
    
    [Fact]
    public async Task RecuperarPorId_DeveRetornarRecursoViewModel()
    {
        // Arrange
        var recurso = new Recurso("Rec1", "Descricao1");
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
        var consumidor = new Consumidor("Consumidor1", "Descricao1");
        var recursoConsumidor = new RecursoConsumidor(recurso.Id, consumidor.Id, EnumStatusRecursoConsumidor.Habilitado);

        recurso.RecursoConsumidores.Add(recursoConsumidor);
        consumidor.RecursoConsumidores.Add(recursoConsumidor);

        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdentificacaoAsync(It.IsAny<string>())).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.RecuperarPorIdentificacaoAsync(It.IsAny<string>())).ReturnsAsync(consumidor);
        _repRecursoConsumidorMemory.Setup(rc => rc.RecuperarPorCodigoRecursoEConsumidorAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(recursoConsumidor);

        // Act
        var result = await _aplicRecurso.VerificaRecursoHabilitado(recurso.Identificacao, consumidor.Identificacao);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Recurso1", result.IdentificacaoRecurso);
        Assert.Equal("Descricao1", result.DescricaoRecurso);
        Assert.Equal("Consumidor1", result.IdentificacaoConsumidor);
    }
    
    [Fact]
    public async Task AlterarPercentualDeLiberacaoDeRecurso_RecursoNaoEncontrado_DeveLancarExcecao()
    {
        // Arrange
        var dto = new AlterarPercentualDeLiberacaoRecursoDto(1, 50) { CodigoRecurso = 1, PercentualLiberacao = 50 };
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(dto.CodigoRecurso)).ReturnsAsync((Recurso)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(dto));
        Assert.Equal("Recurso com ID 1 não encontrado.", exception.Message);
    }

    [Fact]
    public async Task AlterarPercentualDeLiberacaoDeRecurso_Quando100EstaoHabilitadosENovoPercentualEh50_DeveDesabilitar50()
    {
        // Arrange
        var dto = new AlterarPercentualDeLiberacaoRecursoDto(1, 50);
        var recurso = new Recurso("Recurso1", "Recurso1");
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Consumidor1", "Consumidor1"), 
            new Consumidor("Consumidor2", "Consumidor2"),
            new Consumidor("Consumidor3", "Consumidor3"),
            new Consumidor("Consumidor4", "Consumidor4")

        };
        var recursoConsumidores = new List<RecursoConsumidor>
        {
            new RecursoConsumidor(recurso.Id, consumidores[0].Id, EnumStatusRecursoConsumidor.Habilitado),
            new RecursoConsumidor(recurso.Id, consumidores[1].Id, EnumStatusRecursoConsumidor.Habilitado),
            new RecursoConsumidor(recurso.Id, consumidores[2].Id, EnumStatusRecursoConsumidor.Habilitado),
            new RecursoConsumidor(recurso.Id, consumidores[3].Id, EnumStatusRecursoConsumidor.Habilitado),
        };

        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(dto.CodigoRecurso)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(r => r.RecuperarTodos()).Returns(consumidores.AsQueryable());
        _repRecursoConsumidorMemory.Setup(r => r.RecuperarTodosPorCodigoRecursoAsync(recurso.Id)).Returns(recursoConsumidores.AsQueryable());
        _repRecursoConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>()))
            .Returns(Task.CompletedTask);
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDTO>())).ReturnsAsync(1);
        
        
        // Act
        await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(dto);

        // Assert
        var quantidadeTotalConsumidores = consumidores.Count;
        var quantidadeDesejada = (int)Math.Floor(quantidadeTotalConsumidores * dto.PercentualLiberacao / 100);

        var quantidadeHabilitados = recursoConsumidores.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var quantidadeDesabilitados = recursoConsumidores.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Desabilitado);

        Assert.Equal(quantidadeDesejada, quantidadeHabilitados);
        Assert.Equal(quantidadeTotalConsumidores - quantidadeDesejada, quantidadeDesabilitados);
        _repRecursoConsumidorMemory.Verify(r => r.AlterarAsync(It.IsAny<RecursoConsumidor>()), Times.AtLeastOnce);
        _repRecursoConsumidorMemory.Verify(a => a.InserirAsync(It.IsAny<RecursoConsumidor>()), Times.Never);
    }

     [Fact]
    public async Task teste_id()
    {
        // Arrange
        var dto = new AlterarPercentualDeLiberacaoRecursoDto(1, 75);
        var recurso = new Recurso("Recurso1", "Recurso1");
        var consumidores = new List<Consumidor>
        {
            new Consumidor("Consumidor1", "Consumidor1"), 
            new Consumidor("Consumidor2", "Consumidor2"),
            new Consumidor("Consumidor3", "Consumidor3"),
            new Consumidor("Consumidor4", "Consumidor4")

        };
        var recursoConsumidores = new List<RecursoConsumidor>
        {
            new RecursoConsumidor(recurso.Id, consumidores[0].Id, EnumStatusRecursoConsumidor.Desabilitado),
            new RecursoConsumidor(recurso.Id, consumidores[1].Id, EnumStatusRecursoConsumidor.Desabilitado),
            new RecursoConsumidor(recurso.Id, consumidores[2].Id, EnumStatusRecursoConsumidor.Desabilitado),
            new RecursoConsumidor(recurso.Id, consumidores[3].Id, EnumStatusRecursoConsumidor.Desabilitado),
        };

        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(dto.CodigoRecurso)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(r => r.RecuperarTodos()).Returns(consumidores.AsQueryable());
        _repRecursoConsumidorMemory.Setup(r => r.RecuperarTodosPorCodigoRecursoAsync(recurso.Id)).Returns(recursoConsumidores.AsQueryable());
        _repRecursoConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>()))
            .Returns(Task.CompletedTask);
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDTO>())).ReturnsAsync(1);
        
        
        // Act
        await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(dto);

        // Assert
        var quantidadeTotalConsumidores = consumidores.Count;
        var quantidadeDesejada = (int)Math.Floor(quantidadeTotalConsumidores * dto.PercentualLiberacao / 100);

        var quantidadeHabilitados = recursoConsumidores.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var quantidadeDesabilitados = recursoConsumidores.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Desabilitado);

        Assert.Equal(quantidadeDesejada, quantidadeHabilitados);
        Assert.Equal(quantidadeTotalConsumidores - quantidadeDesejada, quantidadeDesabilitados);
        _repRecursoConsumidorMemory.Verify(r => r.AlterarAsync(It.IsAny<RecursoConsumidor>()), Times.AtLeastOnce);
        _repRecursoConsumidorMemory.Verify(a => a.InserirAsync(It.IsAny<RecursoConsumidor>()), Times.Never);
    }

    
    [Fact]
    public async Task Alterar_RecursoExistente()
    {
        // Arrange
        var recurso = new Recurso("Ident1", "Desc1");

        var inputModel = new AlterarRecursoDTO("IdentAlterada", "DescAlterada");
    
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