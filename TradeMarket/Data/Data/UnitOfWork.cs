using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new CustomerRepository(context);
                return _customerRepository;
            }
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                if (_personRepository == null)
                    _personRepository = new PersonRepository(context);
                return _personRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(context);
                return _productRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                if (_productCategoryRepository == null)
                    _productCategoryRepository = new ProductCategoryRepository(context);
                return _productCategoryRepository;
            }
        }

        public IReceiptRepository ReceiptRepository
        {
            get
            {
                if (_receiptRepository == null)
                    _receiptRepository = new ReceiptRepository(context);
                return _receiptRepository;
            }
        }

        public IReceiptDetailRepository ReceiptDetailRepository
        {
            get
            {
                if(_receiptDetailRepository == null)
                    _receiptDetailRepository = new ReceiptDetailRepository(context);
                return _receiptDetailRepository;
            }
        }

        private readonly TradeMarketDbContext context;

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public UnitOfWork(TradeMarketDbContext context)
        {
            this.context = context;
        }

        private ICustomerRepository _customerRepository;
        private IPersonRepository _personRepository;
        private IProductRepository _productRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IReceiptRepository _receiptRepository;
        private IReceiptDetailRepository _receiptDetailRepository;
    }
}
