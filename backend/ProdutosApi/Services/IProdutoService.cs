using ProdutosApi.Dtos;

namespace ProdutosApi.Services;

public interface IProdutoService
{
    Task<PagedResult<ProdutoDto>> ListarAsync(
        string? nome, long? categoriaId, int pagina, int tamanhoPagina);
}