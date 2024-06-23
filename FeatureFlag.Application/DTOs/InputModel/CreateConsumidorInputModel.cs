namespace FeatureFlag.Application.DTOs;

public record CreateConsumidorInputModel(string Identificacao, string Descricao, List<CreateRecursoInputModel> Recursos, List<CreateRecursoConsumidorInputModel> RecursoConsumidor);