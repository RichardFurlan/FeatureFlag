namespace FeatureFlag.Application.DTOs;

public record UpdateConsumidorInputModel(string? Identificacao, string? Descricao, List<UpdateRecursoInputModel>? Recursos);