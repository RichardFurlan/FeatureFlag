using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecursosPorConsumidorViewModel
{

    public RecursosPorConsumidorViewModel(string identificacaoConsumidor, List<RecursosStatusViewModel> recursosStatus)
    {
        IdentificacaoConsumidor = identificacaoConsumidor;
        RecursosStatus = recursosStatus;
    }

    public string IdentificacaoConsumidor { get; init; }
    public List<RecursosStatusViewModel> RecursosStatus { get; init; }
    
};



