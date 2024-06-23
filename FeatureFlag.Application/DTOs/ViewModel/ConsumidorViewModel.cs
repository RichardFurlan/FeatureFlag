namespace FeatureFlag.Application.DTOs.ViewModel;

public record ConsumidorViewModel
{
    public ConsumidorViewModel(string identificador, string descricao, List<RecursoViewModel> recursos)
    {
        
        Identificador = identificador;
        Descricao = descricao;
        Recursos = recursos;
    }
    public string Identificador { get; init; }
    public string Descricao { get; init; }
    public List<RecursoViewModel> Recursos { get; init; }
    
};