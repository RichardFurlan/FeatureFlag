namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarConsumidorDTO
{
    public RecuperarConsumidorDTO(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }
    

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
    
};