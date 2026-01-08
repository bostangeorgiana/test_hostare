using CampusEats.Persistence.Repositories;

namespace CampusEats.Features.Loyalty.GetLoyaltyPoints;

public class GetLoyaltyPointsHandler
{
    private readonly StudentRepository _studentRepository;

    public GetLoyaltyPointsHandler(StudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<LoyaltyResponse> HandleAsync(GetLoyaltyPointsQuery query)
    {
        var student = await _studentRepository.GetByIdAsync(query.StudentId);

        if (student == null)
            throw new Exception("Student not found.");

        return new LoyaltyResponse
        {
            StudentId = student.StudentId,
            LoyaltyPoints = student.LoyaltyPoints
        };
    }
}