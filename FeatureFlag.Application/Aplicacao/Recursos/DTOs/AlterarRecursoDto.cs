namespace FeatureFlag.Application.DTOs.InputModel;

public record AlterarRecursoDto(string? Identificacao, string? Descricao, List<AlterarConsumidorDto>? Consumidores);