using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.InputModel;

public record CriarRecursoConsumidorDto(int CodigoRecurso, int CodigoConsumidor, EnumStatusRecursoConsumidor Status);