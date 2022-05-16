using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService service)
        {
            _receiptService = service;
        }

        // GET: api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            return Ok(await _receiptService.GetAllAsync());
        }

        // GET: api/receipts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            var receipt = await _receiptService.GetByIdAsync(id);

            if (receipt is null)
                return NotFound(id);

            return Ok(receipt);
        }

        // GET: api/receipts/1/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetByIdDetails(int id)
        {
            var receiptDetails = await _receiptService.GetReceiptDetailsAsync(id);

            if (receiptDetails is null)
                return NotFound(id);

            return Ok(receiptDetails);
        }

        // GET: api/receipts/1/sum
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetSumReceiptById(int id)
        {
            var receiptDetails = await _receiptService.GetReceiptDetailsAsync(id);
            var sum = receiptDetails.Sum(rd => rd.DiscountUnitPrice * rd.Quantity);

            return Ok(sum);
        }

        // GET: api/receipts/period
        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceiptsByPeriod(DateTime startDate, DateTime endDate)
        {
            var allReceipts = await _receiptService.GetAllAsync();
            var receipts = allReceipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);

            return Ok(receipts);
        }

        // POST: api/receipts
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ReceiptModel value)
        {
            try
            {
                await _receiptService.AddAsync(value);
                return Created("", value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ReceiptModel value)
        {
            try
            {
                await _receiptService.UpdateAsync(value);
                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/1/products/add/1/1
        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProductToReceipt(int id, int productId, int quantity)
        {
            await _receiptService.AddProductAsync(productId, id, quantity);

            return Ok();
        }

        // PUT: api/receipts/1/products/remove/1/1
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProductFromReceipt(int id, int productId, int quantity)
        {
            await _receiptService.RemoveProductAsync(productId, id, quantity);
            
            return Ok();
        }

        // PUT: api/receipts/1/checkout
        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> Checkout(int id)
        {
            var receipt = await _receiptService.GetByIdAsync(id);
            receipt.IsCheckedOut = true;
            await _receiptService.UpdateAsync(receipt);

            return Ok();
        }

        // DELETE: api/receipts/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _receiptService.DeleteAsync(id);
            return Ok();
        }
    }
}
