namespace CampusEats.Features.Loyalty.GetLoyaltyPoints;

public class GetLoyaltyPointsQuery
{
    public int StudentId { get; set; }

    public GetLoyaltyPointsQuery(int studentId)
    {
        StudentId = studentId;
    }
}