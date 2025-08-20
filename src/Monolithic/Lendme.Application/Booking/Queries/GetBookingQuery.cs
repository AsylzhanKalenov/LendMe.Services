using AutoMapper;
using Lendme.Application.Booking.Dto;
using Lendme.Core.Interfaces.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Queries;

public class GetBookingQuery : IRequest<BookingDto>
{
    public Guid Id { get; set; }
    
    public class Handler : IRequestHandler<GetBookingQuery, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public Handler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<BookingDto> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var res = await _bookingRepository.GetBookingByIdAsync(request.Id, cancellationToken);
            
            return _mapper.Map<BookingDto>(res);
        }
    }
}