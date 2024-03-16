using System.Net.Http.Headers;
using Yggdrasil.Net.Http;

namespace Yggdrasil.AspNetCore.Http;

public class PassThroughAuthorizationHeaderValueProvider : IAuthorizationHeaderValueProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public PassThroughAuthorizationHeaderValueProvider(
        IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Task<AuthenticationHeaderValue?> GetAuthorizationHeaderValueAsync(
        CancellationToken cancellationToken)
    {
        var requestHeaderValue = _contextAccessor.HttpContext
            ?.Request.Headers.Authorization.SingleOrDefault();

        var result = string.IsNullOrEmpty(requestHeaderValue)
            ? null : AuthenticationHeaderValue.Parse(requestHeaderValue);

        return Task.FromResult(result);
    }
}
