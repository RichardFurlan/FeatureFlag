using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs;

public record UpdateRecursoConsumidorInputModel(int? IdRecurso, int? IdConsumidor, EnumStatusRecursoConsumidor? Status);