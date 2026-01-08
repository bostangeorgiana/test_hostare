using CampusEats.Features.Admin.ManageAdmin;
using CampusEats.Features.Admin.ManageAdmin.CreateAdmin;
using CampusEats.Features.Admin.ManageKitchenStaff;
using CampusEats.Features.Admin.ManageStudent;

namespace CampusEats.Features.Admin;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
         var group = app.MapGroup("/admin")
             .RequireAuthorization(policy => policy.RequireRole("admin"))
             .WithTags("Admin");
         
        // Kitchen Staff Management
        group.MapPost("/kitchen-staff", CreateKitchenStaffEndpoint.Handle)
            .WithName("CreateKitchenStaff")
            .WithOpenApi();

        group.MapGet("/kitchen-staff", GetKitchenStaffEndpoint.Handle)
            .WithName("GetKitchenStaff")
            .WithOpenApi();

        group.MapDelete("/kitchen-staff/{id}", DeleteKitchenStaffEndpoint.Handle)
            .WithName("DeleteKitchenStaff")
            .WithOpenApi();

        // Student Requests Management
        group.MapGet("/students/requests", GetStudentRequestsEndpoint.Handle)
            .WithName("GetStudentRequests")
            .WithOpenApi();

        group.MapPut("/students/requests/{id}/approve", ApproveStudentRequestEndpoint.Handle)
            .WithName("ApproveStudentRequest")
            .WithOpenApi();

        group.MapPut("/students/requests/{id}/reject", RejectStudentRequestEndpoint.Handle)
            .WithName("RejectStudentRequest")
            .WithOpenApi();
        
        group.MapGet("/students", GetStudentsAdminEndpoint.HandleList)
            .WithName("GetStudentsList")
            .WithOpenApi();
        
        group.MapGet("/students/{id}", GetStudentsAdminEndpoint.HandleById)
            .WithName("GetStudentsById")
            .WithOpenApi();

        // Admin Management
        group.MapPost("/create-admin", CreateAdminEndpoint.Handle)
            .WithName("CreateAdmin")
            .WithOpenApi();

        group.MapDelete("/{id}", DeleteAdminEndpoint.Handle)
            .WithName("DeleteAdmin")
            .WithOpenApi();
        
        group.MapGet("/admins", GetAdminsEndpoint.Handle)
            .WithName("GetAdmins")
            .WithOpenApi();

        // Orders Statistics
        group.MapGet("/orders/today-count", GetTodayOrdersCountEndpoint.Handle)
            .WithName("GetTodayOrdersCount")
            .WithOpenApi();

        return app;
    }
}