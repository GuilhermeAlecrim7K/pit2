using System.Text.Json;

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
    public async Task<ActionResult<List<Product>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var totalRecords = await _context.Products.CountAsync();
        List<Product> products =
            await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var paginationMetadata = new
        {
            totalRecords,
            totalPages,
            currentPage = pageNumber,
            pageSize
        };
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProductById(Guid id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var dbProduct = new Product()
        {
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

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Guid id, Product product)
    {
        Product? dbProduct = await _context.Products.FindAsync(id);
        if (dbProduct is null)
            return NotFound();
        dbProduct.Name = product.Name;
        dbProduct.Description = product.Description;
        dbProduct.IsActive = product.IsActive;
        dbProduct.Price = product.Price;
        dbProduct.LastUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Product? dbProduct = await _context.Products.FindAsync(id);
        if (dbProduct is null)
            return NotFound();
        _context.Products.Remove(dbProduct);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}