using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.InputModel;

public record CreateRecursoConsumidorInputModel(int CodigoRecurso, int CodigoConsumidor, EnumStatusRecursoConsumidor Status);