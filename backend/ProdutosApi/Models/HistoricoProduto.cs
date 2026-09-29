namespace ProdutosApi.Models;

public class HistoricoProduto
{
    public long Id { get; set; }
    public long ProdutoId { get; set; }
    public Produto? Produto { get; set; }
    public string CampoAlterado { get; set; } = string.Empty;
    public string? ValorAntigo { get; set; }
    public string? ValorNovo { get; set; }
    public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
}