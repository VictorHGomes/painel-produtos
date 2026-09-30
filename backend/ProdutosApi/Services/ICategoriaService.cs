using ProdutosApi.Dtos;

namespace ProdutosApi.Services;

public interface ICategoriaService
{
    Task<IReadOnlyList<CategoriaDto>> ListarAsync();
}