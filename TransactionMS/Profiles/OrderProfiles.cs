using AutoMapper;
using TransactionMS.Data.Dtos;
using TransactionMS.Models;

namespace TransactionMS.Profiles
{
    public class OrderProfiles:Profile
    {
        public OrderProfiles()
        {
            CreateMap<CreateOrderDTO , Order>().ReverseMap();
            CreateMap<Order ,OrderComprehensiveDTO>().ReverseMap();
            CreateMap<MakeSaleDTO ,Sales>().ReverseMap();
            CreateMap<ProductSales, ProductDTO>().ReverseMap();
        }
    }
}
