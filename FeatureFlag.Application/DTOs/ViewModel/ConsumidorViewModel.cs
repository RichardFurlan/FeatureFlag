namespace FeatureFlag.Application.DTOs.ViewModel;

public record ConsumidorViewModel
{
    public ConsumidorViewModel(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }
    

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
    
};