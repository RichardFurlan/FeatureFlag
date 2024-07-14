using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Domain.Entities;

public class RecursoConsumidor : BaseEntity
{
    public RecursoConsumidor(int? codigoRecurso, int? codigoConsumidor, EnumStatusRecursoConsumidor? status)
    {
        CodigoRecurso = codigoRecurso ?? CodigoRecurso;
        CodigoConsumidor = codigoConsumidor ?? CodigoConsumidor;
        Status = status ?? Status;
    }

    public int CodigoRecurso { get; private set; }
    public int CodigoConsumidor { get; private set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }


    public void Update(RecursoConsumidor recursoConsumidor)
    {
        CodigoRecurso = recursoConsumidor.CodigoRecurso;
        CodigoConsumidor = recursoConsumidor.CodigoConsumidor;
        recursoConsumidor.DefinirStatus(recursoConsumidor.Status);
    }

    public void Desabilitar()
    {
        Status = EnumStatusRecursoConsumidor.Desabilitado;
    }
    
    public void Habilitar()
    {
        Status = EnumStatusRecursoConsumidor.Habilitado;
    }

    public void DefinirStatus(EnumStatusRecursoConsumidor status)
    {
        if (status == EnumStatusRecursoConsumidor.Habilitado)
        {
            Habilitar();
        }
        else
        {
            Desabilitar();
        }
    }
}