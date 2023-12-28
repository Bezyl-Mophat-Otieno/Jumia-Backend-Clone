using AutoMapper;
using ProductMS.Data.Dtos;
using ProductMS.Models;

namespace ProductMS.Profiles
{
    public class ProductProfiles:Profile
    {

        public ProductProfiles()
        {
            CreateMap<AddProductDTO , Product>().ReverseMap();
        }
    }

}
