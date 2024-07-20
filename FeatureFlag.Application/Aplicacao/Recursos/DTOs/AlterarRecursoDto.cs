namespace FeatureFlag.Application.DTOs.InputModel;

public record AlterarRecursoDto
{
    public AlterarRecursoDto(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
};