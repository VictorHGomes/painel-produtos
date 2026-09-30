using Microsoft.EntityFrameworkCore;
using ProdutosApi.Data;
using ProdutosApi.Dtos;

namespace ProdutosApi.Services;

public class ProdutoService : IProdutoService
{
    private readonly AppDbContext _db;

    public ProdutoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<ProdutoDto>> ListarAsync(
        string? nome, long? categoriaId, int pagina, int tamanhoPagina)
    {
        var query = _db.Produtos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(p => EF.Functions.ILike(p.Nome, $"%{nome}%"));

        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId.Value);

        var total = await query.CountAsync();

        var itens = await query
            .OrderBy(p => p.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .Select(p => new ProdutoDto(
                p.Id, p.Nome, p.Descricao, p.Preco, p.Estoque,
                p.CategoriaId, p.Categoria!.Nome,
                p.Ativo, p.DataCriacao, p.DataAtualizacao))
            .ToListAsync();

        return new PagedResult<ProdutoDto>(itens, pagina, tamanhoPagina, total);
    }
}