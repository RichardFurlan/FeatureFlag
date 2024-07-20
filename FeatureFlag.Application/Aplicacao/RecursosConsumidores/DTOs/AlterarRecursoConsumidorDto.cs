using FeatureFlag.Domain.Entities;
using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;

public record AlterarRecursoConsumidorDto
{
    public AlterarRecursoConsumidorDto()
    {
        
    }
    public AlterarRecursoConsumidorDto(int codigoRecurso, int codigoConsumidor, EnumStatusRecursoConsumidor status)
    {
        CodigoRecurso = codigoRecurso;
        CodigoConsumidor = codigoConsumidor;
        Status = status;
    }

    public int CodigoRecurso { get; init; }
    public int CodigoConsumidor { get; init; }
    public EnumStatusRecursoConsumidor Status { get; init; }
    
};