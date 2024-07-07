namespace FeatureFlag.Domain.Entities;

public class Recurso : BaseEntity
{
    public Recurso(string identificacao, string descricao, List<Consumidor>? consumidores, List<RecursoConsumidor>? recursoConsumidores)
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

    public void Update(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }
}