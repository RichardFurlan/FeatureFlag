namespace FeatureFlag.Domain.Entities;

public class Recurso : BaseEntity
{
    public Recurso(string identificacao, string descricao, List<Consumidor>? consumidores = null, List<RecursoConsumidor>? recursoConsumidores = null)
    {
        Identificacao = identificacao;
        Descricao = descricao;
        Consumidores = consumidores ?? new List<Consumidor>();
        RecursoConsumidores = recursoConsumidores ?? new List<RecursoConsumidor>();
    }

    public string Identificacao { get; private set; }
    public string Descricao { get; private set; }
    public List<Consumidor> Consumidores { get; private set; }
    public List<RecursoConsumidor> RecursoConsumidores { get; private set; }

    public void Update(Recurso recurso)
    {
        Identificacao = recurso.Identificacao;
        Descricao = recurso.Descricao;
        Consumidores = recurso.Consumidores;
        RecursoConsumidores = recurso.RecursoConsumidores;
    }
}