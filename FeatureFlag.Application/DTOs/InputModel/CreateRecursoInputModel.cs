namespace FeatureFlag.Application.DTOs;

public record CreateRecursoInputModel(string Identificacao, string Descricao, List<CreateConsumidorInputModel> Consumidores, List<CreateRecursoConsumidorInputModel> RecursosConsumidores);