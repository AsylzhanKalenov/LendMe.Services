using AutoMapper;
using Lendme.Application.Booking.Dto;

namespace Lendme.Application.Booking.Profiles;

public class BookingProfile : Profile
{
    
    public BookingProfile()
    {
        
        CreateMap<Core.Entities.Booking.Booking, BookingDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
        
        CreateMap<Core.Entities.Booking.BookingFinancials, BookingFinancialsDto>();
        CreateMap<Core.Entities.Booking.BookingPayment, BookingPaymentDto>();
        CreateMap<Core.Entities.Booking.BookingStatusHistory, BookingStatusHistoryDto>();
        
        CreateMap<Core.Entities.Booking.ItemHandover, ItemHandoverDto>();
        CreateMap<Core.Entities.Booking.HandoverVerification, HandoverVerificationDto>();
        CreateMap<Core.Entities.Booking.HandoverLocation, HandoverLocationDto>();
        CreateMap<Core.Entities.Booking.ItemCondition, ItemConditionDto>();
        CreateMap<Core.Entities.Booking.DamageReport, DamageReportDto>();
        CreateMap<Core.Entities.Booking.HandoverPhoto, HandoverPhotoDto>();
        CreateMap<Core.Entities.Booking.PhotoMetadata, PhotoMetadataDto>();
        
    }
}