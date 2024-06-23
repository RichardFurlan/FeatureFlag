namespace FeatureFlag.Application.DTOs.ViewModel;

public record RecursoViewModel
{
    public RecursoViewModel(string identificacao, string descricao, List<ConsumidorViewModel> consumidores)
    {
        Identificacao = identificacao;
        Descricao = descricao;
        Consumidores = consumidores;
    }

    public string Identificacao { get; init; }
    public string Descricao { get; init; }
    public List<ConsumidorViewModel> Consumidores { get; init; }
};