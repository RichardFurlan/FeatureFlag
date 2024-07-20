using FeatureFlag.Domain.Entities;

namespace FeatureFlag.Application.DTOs.InputModel;

public record AlterarConsumidorDto
{
    public AlterarConsumidorDto(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
};