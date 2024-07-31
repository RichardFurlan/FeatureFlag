using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursosPorConsumidorDTO
{

    public RecuperarRecursosPorConsumidorDTO(string identificacaoConsumidor, List<RecuperarRecursosStatusDTO> recursosStatus)
    {
        IdentificacaoConsumidor = identificacaoConsumidor;
        RecursosStatus = recursosStatus;
    }

    public string IdentificacaoConsumidor { get; init; }
    public List<RecuperarRecursosStatusDTO> RecursosStatus { get; init; }
    
};



