namespace Lendme.Application.Booking.Dto;

public class DetectedIssueDto
{
    public string Type { get; set; } // Scratch, Dent, Missing Part
    public string Severity { get; set; } // Minor, Major, Critical
    public string Location { get; set; } // Coordinates on image
    public double Confidence { get; set; } // AI confidence score
    public string Description { get; set; }
}