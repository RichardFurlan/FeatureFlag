using FeatureFlag.Application.DTOs;
using FeatureFlag.Application.DTOs.ViewModel;
using FeatureFlag.Domain.Interefaces;

namespace FeatureFlag.Application.Aplicacao;

public class AplicRecurso : IAplicRecurso   
{
    public List<RecursoViewModel> ListarTodos(string query)
    {
        throw new NotImplementedException();
    }

    public RecursoViewModel ListarPorId(int id)
    {
        throw new NotImplementedException();
    }

    public int Inserir(CreateRecursoInputModel createRecursoInputModel)
    {
        throw new NotImplementedException();
    }

    public void Alterar(UpdateRecursoInputModel updateRecursoInputModel)
    {
        throw new NotImplementedException();
    }

    public void Inativar(int id)
    {
        throw new NotImplementedException();
    }
}