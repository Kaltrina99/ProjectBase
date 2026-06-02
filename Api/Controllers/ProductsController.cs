using Application.DTOs;
using Application.Interfaces;
using Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (product is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Product not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"No product exists with id '{id}'."
            });
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Request body is required",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var created = await _productService.CreateAsync(request, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Product {ProductId} was created", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Request body is required",
                Status = StatusCodes.Status400BadRequest
            });
        }

        try
        {
            var updated = await _productService.UpdateAsync(id, request, cancellationToken).ConfigureAwait(false);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Product not found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message
            });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Product not found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message
            });
        }
    }
}
