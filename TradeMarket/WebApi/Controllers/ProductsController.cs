using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService service)
        {
            _productService = service;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get(int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var allProducts = await _productService.GetAllAsync();

            IEnumerable<ProductModel> products;

            if (minPrice != null || maxPrice != null)
            {
                decimal min = minPrice ?? decimal.MinValue;
                decimal max = maxPrice ?? decimal.MaxValue;

                products = allProducts.Where(p => p.Price > min && p.Price < max);
            }
            else products = allProducts.ToList();

            if (categoryId != null)
                products = products.Where(p => p.ProductCategoryId == categoryId);

            return Ok(products);
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if(product is null)
                return NotFound(id);

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel value)
        {
            try
            {
                await _productService.AddAsync(value);
                return Created("", value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductModel value)
        {
            try
            {
                await _productService.UpdateAsync(value);
                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

        // GET: api/products/categories
        [HttpGet("categories/")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            return Ok(await _productService.GetAllProductCategoriesAsync());
        }

        // POST: api/products/categories
        [HttpPost("categories/")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel value)
        {
            try
            {
                await _productService.AddCategoryAsync(value);
                return Created("", value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/products/categories/1
        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] ProductCategoryModel value)
        {
            try
            {
                await _productService.UpdateCategoryAsync(value);
                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/products/categories/1
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _productService.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
