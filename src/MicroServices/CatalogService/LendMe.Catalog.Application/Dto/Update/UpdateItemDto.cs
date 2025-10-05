using Microsoft.AspNetCore.Http;

namespace LendMe.Catalog.Application.Dto.Update;

public class UpdateItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string IdentifyNumber { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal? DepositAmount { get; set; }
    public Guid CategoryId { get; set; }
    public Guid OwnerId { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public List<IFormFile>? Images { get; set; }
}