using AutoMapper;
using BusinessSolutionsTest.Core.Models;
using BusinessSolutionsTest.UI.ViewModels;

namespace BusinessSolutionsTest.UI.Helpers;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<SaveOrderViewModel, Order>()
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => new Provider { Name = src.SelectedProvider }));
        CreateMap<Order, SaveOrderViewModel>()
            .ForMember(dest => dest.SelectedProvider, opt => opt.MapFrom(src => src.Provider.Name))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.Date));
    }
}