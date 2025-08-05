using Lendme.Application.Catalog.Queries.Dto;
using Microsoft.AspNetCore.Http;

namespace Lendme.Application.Catalog.Commands.Dto;

public class AddItemRequest
{
    public string Title { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal DepositAmount { get; set; }
    public Guid CategoryId { get; set; }
    public Guid OwnerId { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    
    public LocationDto Location { get; set; }
    public List<IFormFile> Images { get; set; }
    public RentalTermsDto Terms { get; set; }
}