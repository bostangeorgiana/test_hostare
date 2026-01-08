using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Payment.Process;

public static class ProcessPaymentEndpoint
{
    public static async Task<IResult> Handle(
        [FromBody] ProcessPaymentCommand command,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}