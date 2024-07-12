using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursoConsumidorDto
{
    public RecuperarRecursoConsumidorDto(RecuperarRecursoDto recuperarRecursoDto, RecuperarConsumidorDto recuperarConsumidor, EnumStatusRecursoConsumidor status)
    {
        RecuperarRecursoDto = recuperarRecursoDto;
        RecuperarConsumidor = recuperarConsumidor;
        Status = status;
    }

    public RecuperarRecursoDto RecuperarRecursoDto { get;  init; }
    public RecuperarConsumidorDto RecuperarConsumidor { get; init; }
    public EnumStatusRecursoConsumidor Status { get; init; }
};