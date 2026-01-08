using CampusEats.Persistence.Repositories;
using CampusEats.Features.Loyalty;

namespace CampusEats.Features.Loyalty.AddLoyaltyPoints;

public class AddLoyaltyPointsHandler
{
    private readonly StudentRepository _studentRepository;

    public AddLoyaltyPointsHandler(StudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<LoyaltyResponse> HandleAsync(AddLoyaltyPointsRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId);

        if (student == null)
            throw new Exception("Student not found."); 

        student.LoyaltyPoints += request.PointsToAdd;

        await _studentRepository.UpdateStudentAsync(student);

        return new LoyaltyResponse
        {
            StudentId = student.StudentId,
            LoyaltyPoints = student.LoyaltyPoints
        };
    }
}