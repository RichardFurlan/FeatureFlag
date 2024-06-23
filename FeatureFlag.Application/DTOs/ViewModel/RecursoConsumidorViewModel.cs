using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecursoConsumidorViewModel
{
    public RecursoConsumidorViewModel(RecursoViewModel recurso, ConsumidorViewModel consumidor, EnumStatusRecursoConsumidor status)
    {
        Recurso = recurso;
        Consumidor = consumidor;
        Status = status;
    }

    public RecursoViewModel Recurso { get;  init; }
    public ConsumidorViewModel Consumidor { get; init; }
    public EnumStatusRecursoConsumidor Status { get; init; }
};