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
    public async Task<ActionResult<Product>> GetProductById(Guid id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var dbProduct = new Product()
        {
            Id = new Guid(),
            CreatedAt = DateTime.UtcNow,
            Description = product.Description,
            IsActive = product.IsActive,
            Name = product.Name,
            Price = product.Price,
        };
        dbProduct.LastUpdated = dbProduct.CreatedAt;
        _context.Products.Add(dbProduct);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProductById), new { id = dbProduct.Id }, dbProduct);
    }
}