using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursoAtivoDTO
{
    public RecuperarRecursoAtivoDTO(string identificacaoRecurso, string descricaoRecurso ,string identificacaoConsumidor, bool ativo)
    {
        IdentificacaoRecurso = identificacaoRecurso;
        DescricaoRecurso = descricaoRecurso;
        IdentificacaoConsumidor = identificacaoConsumidor;
        Ativo = ativo;
    }

    public string IdentificacaoRecurso { get; init; }
    public string DescricaoRecurso { get; init; }
    public string IdentificacaoConsumidor { get; init; }
    public bool Ativo { get; init; }
};