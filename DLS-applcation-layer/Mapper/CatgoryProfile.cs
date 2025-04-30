using AutoMapper;
using DLS_Domin_layer.Modules;
using Domain_Layer.DTOs;

namespace DLS.Mapper
{
    public class CatgoryProfile:Profile
    {
        public CatgoryProfile()
        {
            CreateMap<ProductCatgoryViewModel, Category>()
                 .ForMember(item => item.Catagory, opt => opt.MapFrom(item => item.Catagory))
                .ReverseMap();
        }
    }
}
