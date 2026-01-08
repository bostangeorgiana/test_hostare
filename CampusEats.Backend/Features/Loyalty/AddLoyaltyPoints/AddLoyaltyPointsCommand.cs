namespace CampusEats.Features.Loyalty.AddLoyaltyPoints;

public class AddLoyaltyPointsRequest
{
    public int StudentId { get; set; }
    public int PointsToAdd { get; set; }
}