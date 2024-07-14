using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Application.DTOs.InputModel;

public record AlterarConsumidorDto(string? Identificacao, string? Descricao, List<Recurso>? Recursos, List<RecursoConsumidor>? RecursosConsumidores);