using System.ComponentModel.DataAnnotations;

namespace ProdutosApi.Dtos;

public record CriarProdutoDto(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 150 caracteres.")]
    string Nome,

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    string? Descricao,

    [Range(0.01, 999999999.99, ErrorMessage = "O preço deve ser maior que zero.")]
    decimal Preco,

    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    int Estoque,

    [Range(1, long.MaxValue, ErrorMessage = "Informe uma categoria válida.")]
    long CategoriaId
);