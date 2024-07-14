using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;

public record AlterarRecursoConsumidorDto(int? CodigoRecurso, int? CodigoConsumidor, EnumStatusRecursoConsumidor? Status);