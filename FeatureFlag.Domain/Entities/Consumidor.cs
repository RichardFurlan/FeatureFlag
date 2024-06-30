namespace FeatureFlag.Domain.Entities;

public class Consumidor : BaseEntity
{
    public Consumidor(string identificacao, string descricao, List<Recurso>? recursos, List<RecursoConsumidor>? recursoConsumidores)
    {
        Identificacao = identificacao;
        Descricao = descricao;
        Recursos = recursos ?? new List<Recurso>();
        RecursoConsumidores = recursoConsumidores ?? new List<RecursoConsumidor>();
    }

    public string Identificacao { get; private set; }
    public string Descricao { get; private set; }
    public List<Recurso> Recursos { get; private set; }
    public List<RecursoConsumidor> RecursoConsumidores { get; private set; }

    public void Update(string identificacao, string descricao)
    {
        Identificacao = identificacao;
        Descricao = descricao;
    }
    
}