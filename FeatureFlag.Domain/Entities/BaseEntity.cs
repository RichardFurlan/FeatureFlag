namespace FeatureFlag.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        throw new NotImplementedException();
    }

    public int Id { get; private set; }
    public bool Inativo { get; private set; }

    public void Inativar()
    {
        Inativo = true;
    }
}