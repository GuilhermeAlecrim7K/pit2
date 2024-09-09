using Microsoft.AspNetCore.Mvc;

using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Product>> GetAll()
    {
        List<Product> products = [
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Chocolate Cupcake",
                IsActive = true,
                Price = 39.8m,
                CreatedAt = new(2023, 8, 19, 12, 14, 58, 947),
                LastUpdated = new(2023, 8, 19, 12, 14, 58, 947),
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Strawberry Cupcake",
                IsActive = true,
                Price = 53.32m,
                CreatedAt = new(2023, 8, 20, 14, 7, 21, 54),
                LastUpdated = new(2024, 1, 23, 9, 15, 23, 50),
            }
        ];
        return Ok(products);
    }
}