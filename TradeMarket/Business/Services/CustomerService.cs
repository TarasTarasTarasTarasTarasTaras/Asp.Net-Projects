using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Data.Data;
using AutoMapper;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CustomerModel model)
        {
            Validation.CustomerVal.Validate(model);

            Customer customer = _mapper.Map<Customer>(model);
            await _unitOfWork.CustomerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(customers);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<CustomerModel>(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return 
                _mapper.Map<IEnumerable<CustomerModel>>
                (customers
                .Where(c => c.Receipts
                .SelectMany(r => r.ReceiptDetails)
                .Any(rd => rd.ProductId == productId)));
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            Validation.CustomerVal.Validate(model);

            Customer customer = _mapper.Map<Customer>(model);

            var person = await _unitOfWork.PersonRepository.GetByIdAsync(customer.Id);

            person.Name = customer.Person.Name;
            person.Surname = customer.Person.Surname;
            person.BirthDate = customer.Person.BirthDate;

            _unitOfWork.CustomerRepository.Update(customer);
            _unitOfWork.PersonRepository.Update(person);

            await _unitOfWork.SaveAsync();
        }
    }
}
