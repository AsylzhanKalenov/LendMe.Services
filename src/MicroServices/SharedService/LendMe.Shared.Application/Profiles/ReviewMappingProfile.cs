using AutoMapper;
using LendMe.Shared.Application.Reviews.Dto;
using LendMe.Shared.Core.Entities.ReviewService;

namespace LendMe.Shared.Application.Profiles;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<ReviewPhoto, ReviewPhotoDto>();
        CreateMap<OwnerResponse, OwnerResponseDto>();

        CreateMap<ReviewCreateDto, Review>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.HelpfulCount, o => o.Ignore())
            .ForMember(d => d.HelpfulVoters, o => o.Ignore())
            .ForMember(d => d.IsVerifiedRental, o => o.Ignore())
            .ForMember(d => d.Ratings, o => o.Ignore())
            .ForMember(d => d.Response, o => o.Ignore())
            .ForMember(d => d.Type, o => o.MapFrom(s => Enum.Parse<ReviewType>(s.Type)));
    }
}