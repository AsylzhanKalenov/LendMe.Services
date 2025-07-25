using Lendme.Core.Entities.Catalog;
using Lendme.Core.Entities.Profile;

namespace Lendme.Core.Entities;

public class Report
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int ReportedUserId { get; set; }
    public UserProfile ReportedUserProfile { get; set; }
    public int ReportedItemId { get; set; }
    public Item ReportedItem { get; set; }
    public string ReportedReason { get; set; }
    public bool IsResolved { get; set; }
    public DateTimeOffset ReportedDate { get; set; }
    public DateTimeOffset ResolvedDate { get; set; }
}