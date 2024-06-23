using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs;

public record UpdateRecursoConsumidorInputModel(int? CodigoRecurso, int? CodigoConsumidor, EnumStatusRecursoConsumidor? Status);