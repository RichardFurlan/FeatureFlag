using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecuperarRecursosStatusView
{
    public RecuperarRecursosStatusView(string identificacaoRecurso, EnumStatusRecursoConsumidor status)
    {
        IdentificacaoRecurso = identificacaoRecurso;
        Status = status;
    }

    public string IdentificacaoRecurso { get; init; }
    public EnumStatusRecursoConsumidor Status { get; init; }
};