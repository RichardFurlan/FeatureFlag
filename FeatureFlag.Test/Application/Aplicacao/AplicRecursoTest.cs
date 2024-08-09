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
    public async Task AlterarPercentualDeLiberacaoDeRecurso_DeveAlterarPercentualCorretamente50()
    {
        // Arrange
        var recursoId = 1;
        var percentualLiberacao = 50.0m;

        var alterarPercentualRecursoDto = new AlterarPercentualDeLiberacaoRecursoDto(recursoId, percentualLiberacao);

        var consumidores = new List<Consumidor>
        {
            new Consumidor("Cons1", "Desc1"),
            new Consumidor("Cons2", "Desc2"),
            new Consumidor("Cons3", "Desc3"),
            new Consumidor("Cons4", "Desc4")
        };
        
        
        var recurso = new Recurso("Recurso1", "Descricao1", consumidores);
        
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(recursoId)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.RecuperarTodos()).Returns(consumidores.AsQueryable());
        _repRecursoConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>())).Returns(Task.CompletedTask);

        var recursoConsumidorList = new List<CriarRecursoConsumidorDTO>();
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDTO>()))
            .Callback<CriarRecursoConsumidorDTO>(rc => recursoConsumidorList.Add(rc))
            .ReturnsAsync(1);

        // Act
        await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(alterarPercentualRecursoDto);

        // Assert
        _repRecursoMockMemory.Verify(r => r.RecuperarPorIdAsync(recursoId), Times.Once);
        _repConsumidorMemory.Verify(c => c.RecuperarTodos(), Times.Once);
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(percentualLiberacao, percentualHabilitado, precision: 0);

        _repRecursoConsumidorMemory.Verify(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>()), Times.Exactly(recurso.RecursoConsumidores.Count));
        _aplicRecursoConsumidor.Verify(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDTO>()), Times.Exactly(consumidores.Count - recurso.RecursoConsumidores.Count));
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
            new Consumidor("Cons1", "Desc1"),
            new Consumidor("Cons2", "Desc2"),
            new Consumidor("Cons3", "Desc3"),
            new Consumidor("Cons4", "Desc4")
        };
        
        
        var recurso = new Recurso("Recurso1", "Descricao1", consumidores);
        
        _repRecursoMockMemory.Setup(r => r.RecuperarPorIdAsync(recursoId)).ReturnsAsync(recurso);
        _repConsumidorMemory.Setup(c => c.RecuperarTodos()).Returns(consumidores.AsQueryable());
        _repRecursoConsumidorMemory.Setup(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>())).Returns(Task.CompletedTask);

        var recursoConsumidorList = new List<CriarRecursoConsumidorDTO>();
        _aplicRecursoConsumidor.Setup(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDTO>()))
            .Callback<CriarRecursoConsumidorDTO>(rc => recursoConsumidorList.Add(rc))
            .ReturnsAsync(1);

        // Act
        await _aplicRecurso.AlterarPercentualDeLiberacaoDeRecurso(alterarPercentualRecursoDto);

        // Assert
        _repRecursoMockMemory.Verify(r => r.RecuperarPorIdAsync(recursoId), Times.Once);
        _repConsumidorMemory.Verify(c => c.RecuperarTodos(), Times.Once);
        
        var habilitados = recursoConsumidorList.Count(rc => rc.Status == EnumStatusRecursoConsumidor.Habilitado);
        var percentualHabilitado = (decimal)habilitados / consumidores.Count * 100;

        Assert.Equal(percentualLiberacao, percentualHabilitado, precision: 0);

        _repRecursoConsumidorMemory.Verify(rc => rc.AlterarAsync(It.IsAny<RecursoConsumidor>()), Times.Exactly(recurso.RecursoConsumidores.Count));
        _aplicRecursoConsumidor.Verify(rc => rc.InserirAsync(It.IsAny<CriarRecursoConsumidorDTO>()), Times.Exactly(consumidores.Count - recurso.RecursoConsumidores.Count));
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