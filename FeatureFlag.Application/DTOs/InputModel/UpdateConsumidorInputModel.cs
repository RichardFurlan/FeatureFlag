namespace FeatureFlag.Application.DTOs.InputModel;

public record UpdateConsumidorInputModel(string? Identificacao, string? Descricao, List<UpdateRecursoInputModel>? Recursos);