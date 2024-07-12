namespace FeatureFlag.Application.DTOs.InputModel;

public record AlterarConsumidorDto(string? Identificacao, string? Descricao, List<AlterarRecursoDto>? Recursos, List<AlterarRecursoConsumidorDto>? RecursosConsumidores);