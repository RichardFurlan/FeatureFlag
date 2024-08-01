namespace FeatureFlag.Application.Consumidores.DTOs;

public record RecuperarConsumidorView
{
    public RecuperarConsumidorView(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }
    

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
    
};