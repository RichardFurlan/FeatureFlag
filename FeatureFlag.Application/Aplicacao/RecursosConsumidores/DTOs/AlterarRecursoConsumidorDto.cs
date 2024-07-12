using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs;

public record AlterarRecursoConsumidorDto(int? CodigoRecurso, int? CodigoConsumidor, EnumStatusRecursoConsumidor? Status);