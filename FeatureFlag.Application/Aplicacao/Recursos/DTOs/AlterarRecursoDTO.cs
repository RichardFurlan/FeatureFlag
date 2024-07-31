namespace FeatureFlag.Application.DTOs.InputModel;

public record AlterarRecursoDTO
{
    public AlterarRecursoDTO(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
};