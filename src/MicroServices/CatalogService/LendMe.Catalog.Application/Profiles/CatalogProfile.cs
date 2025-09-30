using AutoMapper;
using LendMe.Catalog.Application.Queries.Dto;
using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Application.Profiles;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<Category, CategoryDto>();

        CreateMap<Rent, RentDto>();

        // Item mappings
        CreateMap<Item, ItemDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        // ItemDetails mappings
        CreateMap<ItemDetails, ItemDetailsDto>();

        // Location mappings
        CreateMap<Location, LocationDto>()
            .ForMember(d => d.Type, opt => opt.MapFrom(s => "Point"))
            .ForMember(d => d.Coordinates, opt => opt.MapFrom(s => s.GetCoordinates()));

        // RentalTerms mappings
        CreateMap<RentalTerms, RentalTermsDto>();
    }
}