using System.Net.Http;
using System.Net.Http.Headers;

namespace CampusEats.Frontend.Services.Auth;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public AuthMessageHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
        this.InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
