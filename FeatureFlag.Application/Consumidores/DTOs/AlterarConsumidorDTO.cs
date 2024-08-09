namespace FeatureFlag.Application.Consumidores.DTOs;

public record AlterarConsumidorDTO
{
    public AlterarConsumidorDTO(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
};