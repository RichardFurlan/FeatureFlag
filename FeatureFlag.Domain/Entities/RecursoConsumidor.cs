using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Domain.Entities;

public class RecursoConsumidor : BaseEntity
{
    public RecursoConsumidor(int codigoRecurso, int codigoConsumidor, EnumStatusRecursoConsumidor status)
    {
        CodigoRecurso = codigoRecurso;
        CodigoConsumidor = codigoConsumidor;
        Status = status;
    }

    public int CodigoRecurso { get; private set; }
    public int CodigoConsumidor { get; private set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }
}