using CampusEats.Shared;
using MediatR;

namespace CampusEats.Features.Auth.RefreshToken;

public record RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>;