using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Persistence.Configurations;

public class RecursoConsumidorConfig : IEntityTypeConfiguration<RecursoConsumidor>
{
    public void Configure(EntityTypeBuilder<RecursoConsumidor> builder)
    {
        throw new NotImplementedException();
    }
}