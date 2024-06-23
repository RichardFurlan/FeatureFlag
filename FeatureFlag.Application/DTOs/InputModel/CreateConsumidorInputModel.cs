namespace FeatureFlag.Application.DTOs.InputModel;

public record CreateConsumidorInputModel(string Identificacao, string Descricao, List<CreateRecursoInputModel> Recursos, List<CreateRecursoConsumidorInputModel> RecursoConsumidor);