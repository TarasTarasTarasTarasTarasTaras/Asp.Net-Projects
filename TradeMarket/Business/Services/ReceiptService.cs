using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(ReceiptModel model)
        {
            Validation.ReceiptVal.Validate(model);

            Receipt receipt = _mapper.Map<Receipt>(model);
            await _unitOfWork.ReceiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;

            _unitOfWork.ReceiptRepository.Update(receipt);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);

            foreach(var receiptDetail in receipt.ReceiptDetails)
                _unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(receipts);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ReceiptModel>(receipt);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return _mapper.Map<IEnumerable<ReceiptDetailModel>>(receipt.ReceiptDetails);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(
                receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate));
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            if (receipt is null)
                throw new MarketException("Receipt with such id does not exist");

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product is null)
                throw new MarketException("Product with such id does not exist");

            var receiptDetail = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);

            if (receiptDetail is null)
            {

                receiptDetail = new ReceiptDetail
                {
                    ProductId = productId,
                    Product = product,
                    ReceiptId = receiptId,
                    Receipt = receipt,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    DiscountUnitPrice = product.Price
                };

                await _unitOfWork.ReceiptDetailRepository.AddAsync(receiptDetail);
            }
            else
            {
                receiptDetail.Quantity += quantity;
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            var receiptDetail = receipt.ReceiptDetails.First(rd => rd.ProductId == productId);

            receiptDetail.Quantity -= quantity;

            if (receiptDetail.Quantity == 0)
                _unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);

            await _unitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return receipt.ReceiptDetails.Sum(rd => rd.Quantity * rd.DiscountUnitPrice);
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var receipt = _mapper.Map<Receipt>(model);
            _unitOfWork.ReceiptRepository.Update(receipt);
            await _unitOfWork.SaveAsync();
        }
    }
}
