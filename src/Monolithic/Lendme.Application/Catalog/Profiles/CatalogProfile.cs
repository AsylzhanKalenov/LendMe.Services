using AutoMapper;
using Lendme.Application.Catalog.Queries.Dto;
using Lendme.Core.Entities.Catalog;

namespace Lendme.Application.Catalog.Profiles;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<Category, CategoryDto>();
        
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