using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WBAPI.Models;
using WBAPI.Services;

namespace WBAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<Product>> Get()
    {
        return Ok(_productService.GetAll());
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<Product> Get(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public ActionResult<Product> Post([FromBody] Product product)
    {
        _productService.Add(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult Put(int id, [FromBody] Product product)
    {
        var existingProduct = _productService.GetById(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        _productService.Update(id, product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult Delete(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        _productService.Delete(id);
        return NoContent();
    }
}
