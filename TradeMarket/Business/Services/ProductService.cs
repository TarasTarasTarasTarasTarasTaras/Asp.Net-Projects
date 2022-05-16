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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(ProductModel model)
        {
            Validation.ProductVal.Validate(model);

            Product product = _mapper.Map<Product>(model);
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation.ProductCategoryVal.Validate(categoryModel);

            ProductCategory productCategory = _mapper.Map<ProductCategory>(categoryModel);
            await _unitOfWork.ProductCategoryRepository.AddAsync(productCategory);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(productCategories);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var allProducts = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            
            var products = 
                allProducts
                .Where(product => 
                    (filterSearch?.CategoryId ?? product.ProductCategoryId) == product.ProductCategoryId && 
                    (filterSearch?.MinPrice ?? product.Price) <= product.Price &&
                    (filterSearch?.MaxPrice ?? product.Price) >= product.Price).ToList();

            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(product);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            Validation.ProductVal.Validate(model);

            Product product = _mapper.Map<Product>(model);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation.ProductCategoryVal.Validate(categoryModel);

            ProductCategory productCategory = _mapper.Map<ProductCategory>(categoryModel);
            _unitOfWork.ProductCategoryRepository.Update(productCategory);
            await _unitOfWork.SaveAsync();
        }
    }
}
