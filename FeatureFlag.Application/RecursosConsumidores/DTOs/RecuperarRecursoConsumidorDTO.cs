using FeatureFlag.Application.Consumidores.DTOs;
using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursoConsumidorDTO
{
    public RecuperarRecursoConsumidorDTO(RecuperarRecursoView recuperarRecursoView, RecuperarConsumidorView recuperarConsumidor, EnumStatusRecursoConsumidor status)
    {
        RecuperarRecursoView = recuperarRecursoView;
        RecuperarConsumidor = recuperarConsumidor;
        Status = status;
    }

    public RecuperarRecursoView RecuperarRecursoView { get;  init; }
    public RecuperarConsumidorView RecuperarConsumidor { get; init; }
    public EnumStatusRecursoConsumidor Status { get; init; }
};