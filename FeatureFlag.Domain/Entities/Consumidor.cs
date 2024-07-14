namespace FeatureFlag.Domain.Entities;

public class Consumidor : BaseEntity
{
    public Consumidor(string identificacao, string descricao, List<Recurso>? recursos = null, List<RecursoConsumidor>? recursoConsumidores = null)
    {
        Identificacao = identificacao ?? throw new ArgumentNullException(nameof(identificacao));
        Descricao = descricao ?? throw new ArgumentNullException(nameof(descricao));
        Recursos = recursos ?? new List<Recurso>();
        RecursoConsumidores = recursoConsumidores ?? new List<RecursoConsumidor>();
    }

    public string Identificacao { get; private set; }
    public string Descricao { get; private set; }
    public List<Recurso> Recursos { get; private set; }
    public List<RecursoConsumidor> RecursoConsumidores { get; private set; }

    public void Update(Consumidor consumidor)
    {
        Identificacao = consumidor.Identificacao;
        Descricao = consumidor.Descricao;
        Recursos = consumidor.Recursos;
        RecursoConsumidores = consumidor.RecursoConsumidores;
    }
    
}