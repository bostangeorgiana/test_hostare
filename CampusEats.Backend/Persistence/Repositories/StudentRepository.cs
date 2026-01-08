using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Persistence.Repositories;

public class StudentRepository(CampusEatsDbContext dbContext)
{
    /// <summary>
    /// Creates a Student profile with loyalty points = 0.
    /// If a Student already exists with the same ID, the method does nothing.
    /// </summary>
    public async Task CreateStudentAsync(int studentId)
    {
        var existing = await dbContext.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == studentId);

        if (existing != null)
            return;

        var student = new Student
        {
            StudentId = studentId,
            LoyaltyPoints = 0
        };

        dbContext.Students.Add(student);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(Student student)
    {
        dbContext.Students.Update(student);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Student?> GetByIdAsync(int studentId)
    {
        return await dbContext.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == studentId);
    }

    public async Task<List<Student>> GetMultipleByIdsAsync(List<int> ids)
    {
        return await dbContext.Students
            .Where(s => ids.Contains(s.StudentId))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StudentUserJoin>> GetStudentsWithUserInfoAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return await dbContext.Students
            .Join(
                dbContext.Users,
                student => student.StudentId,
                user => user.UserId,
                (student, user) => new StudentUserJoin
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    LoyaltyPoints = student.LoyaltyPoints,
                    Role = user.Role
                }
            )
            .OrderByDescending(s => s.UserId)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<StudentUserJoin?> GetStudentWithUserInfoAsync(int studentId)
    {
        return await dbContext.Students
            .Where(s => s.StudentId == studentId)
            .Join(
                dbContext.Users,
                student => student.StudentId,
                user => user.UserId,
                (student, user) => new StudentUserJoin
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    LoyaltyPoints = student.LoyaltyPoints,
                    Role = user.Role
                }
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<int> CountStudentsAsync()
    {
        return await dbContext.Users
            .CountAsync(u => u.Role == "student");
    }

    public async Task<bool> ReservePointsAsync(int studentId, int points)
    {
        if (points <= 0) return true;

        var result = await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE students
            SET reserved_points = reserved_points + {points}
            WHERE student_id = {studentId} AND (loyalty_points - reserved_points) >= {points}");
        return result > 0;
    }

    public async Task CommitReservedPointsAsync(int studentId, int pointsRedeemed, int pointsEarned)
    {
        var sql = $@"
            UPDATE students
            SET loyalty_points = GREATEST(loyalty_points - {pointsRedeemed} + {pointsEarned}, 0),
                reserved_points = GREATEST(reserved_points - {pointsRedeemed}, 0)
            WHERE student_id = {studentId};";

        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    public async Task ReleaseReservedPointsAsync(int studentId, int points)
    {
        if (points <= 0) return;

        var sql = $@"
            UPDATE students
            SET reserved_points = GREATEST(reserved_points - {points}, 0)
            WHERE student_id = {studentId};";

        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    public async Task<StudentUserJoin?> GetUserProfileAsync(int userId)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null) return null;
        if (user.Role == "student")
        {
            var join = await dbContext.Students
                .Where(s => s.StudentId == userId)
                .Join(
                    dbContext.Users,
                    student => student.StudentId,
                    u => u.UserId,
                    (student, u) => new StudentUserJoin
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        LoyaltyPoints = student.LoyaltyPoints,
                        Role = u.Role
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return join;
        }
        return new StudentUserJoin
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            LoyaltyPoints = 0,
            Role = user.Role
        };
    }
}
