using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursoConsumidorDTO
{
    public RecuperarRecursoConsumidorDTO(RecuperarRecursoDTO recuperarRecursoDto, RecuperarConsumidorDTO recuperarConsumidor, EnumStatusRecursoConsumidor status)
    {
        RecuperarRecursoDto = recuperarRecursoDto;
        RecuperarConsumidor = recuperarConsumidor;
        Status = status;
    }

    public RecuperarRecursoDTO RecuperarRecursoDto { get;  init; }
    public RecuperarConsumidorDTO RecuperarConsumidor { get; init; }
    public EnumStatusRecursoConsumidor Status { get; init; }
};