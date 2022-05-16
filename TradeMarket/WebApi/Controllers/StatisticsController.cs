using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService service)
        {
            _statisticService = service;
        }

        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts(int productCount)
        {
            return Ok(await _statisticService.GetMostPopularProductsAsync(productCount));
        }

        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostFavouriteProducts(int id, int productCount)
        {
            return Ok(await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id));
        }

        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomersPeriodTime(int customerCount, DateTime startDate, DateTime endDate)
        {
            return Ok(await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate));
        }

        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<decimal>> GetIncome(int categoryId, DateTime startDate, DateTime endDate)
        {
            return Ok(await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate));
        }
    }
}
