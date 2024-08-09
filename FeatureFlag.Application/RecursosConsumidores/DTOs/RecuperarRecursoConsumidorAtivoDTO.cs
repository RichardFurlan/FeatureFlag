namespace FeatureFlag.Application.Aplicacao.RecursosConsumidores.DTOs;

public record RecuperarRecursoConsumidorAtivoDTO
{
    public RecuperarRecursoConsumidorAtivoDTO(string identificacaoRecurso, string descricaoRecurso ,string identificacaoConsumidor, bool habilitado)
    {
        IdentificacaoRecurso = identificacaoRecurso;
        DescricaoRecurso = descricaoRecurso;
        IdentificacaoConsumidor = identificacaoConsumidor;
        Habilitado = habilitado;
    }

    public string IdentificacaoRecurso { get; init; }
    public string DescricaoRecurso { get; init; }
    public string IdentificacaoConsumidor { get; init; }
    public bool Habilitado { get; init; }
};