using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursosPorConsumidorDto
{

    public RecuperarRecursosPorConsumidorDto(string identificacaoConsumidor, List<RecuperarRecursosStatusDto> recursosStatus)
    {
        IdentificacaoConsumidor = identificacaoConsumidor;
        RecursosStatus = recursosStatus;
    }

    public string IdentificacaoConsumidor { get; init; }
    public List<RecuperarRecursosStatusDto> RecursosStatus { get; init; }
    
};



