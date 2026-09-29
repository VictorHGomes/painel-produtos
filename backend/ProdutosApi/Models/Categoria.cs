namespace ProdutosApi.Models;
public class Categoria
{
    public long Id {get; set;}
    public string Nome { get; set; } = string.Empty; //valor inicial vazio 
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}