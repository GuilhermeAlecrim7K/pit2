using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;
    public ProductsController(DataContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        List<Product> products = await _context.Products.ToListAsync();
        return Ok(products);
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Product>> Get(Guid id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product is null ? NotFound() : Ok(product);
    }
}