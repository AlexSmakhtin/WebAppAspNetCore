﻿using Microsoft.AspNetCore.Authorization;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MySuperShop.ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IRepository<Product> repo, ILogger<ProductController> logger)
        {
            _logger = logger;
            _productRepo = repo;
        }

        [HttpGet("get")]
        public async Task<ActionResult<Product>> GetProduct(Guid id, CancellationToken token)
        {
            try
            {
                var product = await _productRepo.GetById(id, token);
                return product;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddProduct(Product product, CancellationToken token)
        {
            try
            {
                await _productRepo.Add(product, token);
                return Created();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken token)
        {
            try
            {
                var product = await _productRepo.GetById(id, token);
                await _productRepo.Delete(product, token);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("get_all")]
        public async Task<ActionResult<IReadOnlyCollection<Product>>> GetProducts(CancellationToken token)
        {
            try
            {
                var products = await _productRepo.GetAll(token);
                return Ok(products);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult> UpdateProduct(Product product, CancellationToken token)
        {
            try
            {
                await _productRepo.Update(product, token);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
