using Microsoft.EntityFrameworkCore;
using ProdutosApi.Data;
using ProdutosApi.Dtos;
using ProdutosApi.Models;
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

    public async Task<ProdutoDto?> ObterPorIdAsync(long id)
{
    return await _db.Produtos
        .AsNoTracking()
        .Where(p => p.Id == id)
        .Select(p => new ProdutoDto(
            p.Id, p.Nome, p.Descricao, p.Preco, p.Estoque,
            p.CategoriaId, p.Categoria!.Nome,
            p.Ativo, p.DataCriacao, p.DataAtualizacao))
        .FirstOrDefaultAsync();
}

public async Task<ProdutoDto?> CriarAsync(CriarProdutoDto dto)
{
    var categoriaExiste = await _db.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
    if (!categoriaExiste)
        return null;

    var produto = new Produto
    {
        Nome = dto.Nome.Trim(),
        Descricao = dto.Descricao?.Trim(),
        Preco = dto.Preco,
        Estoque = dto.Estoque,
        CategoriaId = dto.CategoriaId,
        Ativo = true,
        DataCriacao = DateTime.UtcNow
    };

    _db.Produtos.Add(produto);
    await _db.SaveChangesAsync();

    return await ObterPorIdAsync(produto.Id);
}
}