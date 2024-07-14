using FeatureFlag.Domain.Enums;

namespace FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;

public record CriarRecursoConsumidorDto
{
    public int CodigoRecurso { get; set; }
    public int CodigoConsumidor { get; set; }
    public EnumStatusRecursoConsumidor Status { get; set; }

    public CriarRecursoConsumidorDto(int codigoRecurso, int codigoConsumidor, EnumStatusRecursoConsumidor status)
    {
        CodigoRecurso = codigoRecurso;
        CodigoConsumidor = codigoConsumidor;
        Status = status;
    }
};