using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            IEnumerable<Product> products =
                receipts
                .First(r => r.CustomerId == customerId)
                .ReceiptDetails
                .Select(rd => rd.Product)
                .GroupBy(p => p.Id)
                .OrderByDescending((x) => x.Count())
                .Take(productCount)
                .OrderBy((x) => x.Count())
                .Select(p => p.First());

            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            decimal income = 
                receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.ProductCategoryId == categoryId)
                .Sum(rd => rd.Quantity * rd.DiscountUnitPrice);

            return income;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();

            IEnumerable<Product> products =
                receiptDetails
                .OrderByDescending(rd => rd.Quantity)
                .Select(rd => rd.Product)
                .Take(productCount);

            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var customers =
                receipts
                .GroupBy(r => r.Customer)
                .Select(a => new CustomerActivityModel
                {
                    CustomerId = a.Key.Id,
                    CustomerName = $"{a.Key.Person.Name} {a.Key.Person.Surname}",
                    ReceiptSum = receipts
                                    .Where(r => r.CustomerId == a.Key.Id)
                                    .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                                    .SelectMany(r => r.ReceiptDetails)
                                    .Sum(rd => rd.Quantity * rd.DiscountUnitPrice)

                })
                .OrderByDescending(c => c.ReceiptSum)
                .Take(customerCount);

            return customers;
        }
    }
}
