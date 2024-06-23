namespace FeatureFlag.Application.DTOs.InputModel;

public record UpdateRecursoInputModel(string? Identificacao, string? Descricao, List<UpdateConsumidorInputModel>? Consumidores);