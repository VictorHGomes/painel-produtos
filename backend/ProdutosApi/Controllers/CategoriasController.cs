using Microsoft.AspNetCore.Mvc;
using ProdutosApi.Dtos;
using ProdutosApi.Services;

namespace ProdutosApi.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaDto>>> Listar()
    {
        var categorias = await _service.ListarAsync();
        return Ok(categorias);
    }
}