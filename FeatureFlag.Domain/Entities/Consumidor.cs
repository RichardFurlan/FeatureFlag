namespace FeatureFlag.Domain.Entities;

public class Consumidor : BaseEntity
{
    public Consumidor()
    {
        
    }
    public Consumidor(string identificacao, string descricao)
    {
        Identificacao = identificacao ?? throw new ArgumentNullException(nameof(identificacao));
        Descricao = descricao ?? throw new ArgumentNullException(nameof(descricao));
        
        
        Recursos = new List<Recurso>();
        RecursoConsumidores = new List<RecursoConsumidor>();
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