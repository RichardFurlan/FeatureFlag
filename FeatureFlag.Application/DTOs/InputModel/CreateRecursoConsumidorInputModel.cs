using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs;

public record CreateRecursoConsumidorInputModel(int IdRecurso, int IdConsumidor, EnumStatusRecursoConsumidor Status);