using FeatureFlag.Application.DTOs.ViewModel;

namespace FeatureFlag.Application.Consumidores.DTOs;

public record RecuperarRecursosPorConsumidorView
{

    public RecuperarRecursosPorConsumidorView(string identificacaoConsumidor, List<RecuperarRecursosStatusView> recursosStatus)
    {
        IdentificacaoConsumidor = identificacaoConsumidor;
        RecursosStatus = recursosStatus;
    }

    public string IdentificacaoConsumidor { get; init; }
    public List<RecuperarRecursosStatusView> RecursosStatus { get; init; }
    
};



