namespace ProdutosApi.Dtos;
public record ProdutoDto (
    long Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    int Estoque,
    long CategoriaId,
    string CategoriaNome,
    bool Ativo,
    DateTime DataCriacao,
    DateTime? DataAtualizacao
);