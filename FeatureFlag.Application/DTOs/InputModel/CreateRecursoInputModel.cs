namespace FeatureFlag.Application.DTOs.InputModel;

public record CreateRecursoInputModel(string Identificacao, string Descricao, List<CreateConsumidorInputModel> Consumidores, List<CreateRecursoConsumidorInputModel> RecursosConsumidores);