namespace FeatureFlag.Application.DTOs;

public record UpdateRecursoInputModel(string? Identificacao, string? Descricao, List<UpdateConsumidorInputModel>? Consumidores);