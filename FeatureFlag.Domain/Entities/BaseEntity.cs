namespace FeatureFlag.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; private set; }
    public bool Inativo { get; private set; }

    public void Inativar()
    {
        Inativo = true;
    }
}