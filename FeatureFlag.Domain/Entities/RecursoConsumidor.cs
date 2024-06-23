using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Domain.Entities;

public class RecursoConsumidor : BaseEntity
{
    public RecursoConsumidor(int idRecurso, int idConsumidor, EnumStatusRecursoConsumidor status)
    {
        IdRecurso = idRecurso;
        IdConsumidor = idConsumidor;
        Status = status;
    }

    public int IdRecurso { get; private set; }
    public int IdConsumidor { get; private set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }
}