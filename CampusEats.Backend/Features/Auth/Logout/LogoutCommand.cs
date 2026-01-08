using CampusEats.Shared;
using MediatR;

namespace CampusEats.Features.Auth.Logout;

public record LogoutCommand : IRequest<Result<bool>>;