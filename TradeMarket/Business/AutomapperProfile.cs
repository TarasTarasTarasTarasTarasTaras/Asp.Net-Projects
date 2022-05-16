using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.CategoryName, pr => pr.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds, pr => pr.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)));

            CreateMap<CustomerModel, Customer>()
                .ForMember(c => c.Person, cm => cm.MapFrom(x => new Person { Name = x.Name, Surname = x.Surname, BirthDate = x.BirthDate }))
                .ForMember(c => c.DiscountValue, cm => cm.MapFrom(x => x.DiscountValue));

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(pr => pr.Id)))
                .ReverseMap();
        }
    }
}