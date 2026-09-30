using ProdutosApi.Dtos;

namespace ProdutosApi.Services;

public interface IProdutoService
{
    Task<ProdutoDto?> ObterPorIdAsync(long id); //ProdutoDto? pra caso não seja encontrado
    Task<PagedResult<ProdutoDto>> ListarAsync(
        string? nome, long? categoriaId, int pagina, int tamanhoPagina);
    Task<ProdutoDto?> CriarAsync(CriarProdutoDto dto);
}