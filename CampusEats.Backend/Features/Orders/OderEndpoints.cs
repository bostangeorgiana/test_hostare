using CampusEats.Features.Orders.CancelOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CampusEats.Features.Orders.CreateOrder;
using CampusEats.Features.Orders.GetActiveOrders;
using CampusEats.Features.Orders.GetOrderHistory;
using CampusEats.Features.Orders.GetOrderDetails;
using CampusEats.Features.Orders.UpdateOrderStatus;

namespace CampusEats.Features.Orders;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/orders");
        
        group.MapPost("/", async (
            [FromBody] CreateOrderCommand cmd,
            IMediator mediator
        ) =>
        {
            var orderId = await mediator.Send(cmd);
            return Results.Ok(orderId);
        });
        
        group.MapGet("/history/{studentId:int}", async (
            int studentId,
            IMediator mediator
        ) =>
        {
            var result = await mediator.Send(new GetOrderHistoryCommand(studentId));
            return Results.Ok(result);
        });
        
        group.MapGet("/{orderId:int}", async (
            int orderId,
            IMediator mediator
        ) =>
        {
            var result = await mediator.Send(new GetOrderDetailsCommand(orderId));

            return result is null
                ? Results.NotFound(new { Message = "Order not found." })
                : Results.Ok(result);
        });
        
        group.MapPatch("/{orderId}/cancel", async (
            int orderId,
            int studentId,
            IMediator mediator) =>
        {
            var cmd = new CancelOrderCommand(orderId, studentId);
            await mediator.Send(cmd);
            return Results.Ok(new { message = "Order cancelled successfully." });
        });
        
        group.MapPatch("/{orderId}/status", async (
            int orderId,
            string newStatus,
            int staffUserId,
            IMediator mediator
        ) =>
        {
            var cmd = new UpdateOrderStatusCommand(orderId, newStatus, staffUserId);
            await mediator.Send(cmd);
            return Results.Ok(new { message = "Order status updated successfully." });
        });

        group.MapGet("/kitchen/active", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetActiveOrdersCommand());
            return Results.Ok(result);
        });

    }
}