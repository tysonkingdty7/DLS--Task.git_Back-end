using AutoMapper;
using DLS_Domin_layer.Modules;
using Domain_Layer.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductCatgoryViewModel, Category>().ReverseMap();
    }
}
