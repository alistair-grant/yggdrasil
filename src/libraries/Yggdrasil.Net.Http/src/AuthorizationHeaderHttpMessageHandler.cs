namespace Yggdrasil.Net.Http;

public class AuthorizationHeaderHttpMessageHandler : DelegatingHandler
{
    private readonly IAuthorizationHeaderValueProvider _valueProvider;

    public AuthorizationHeaderHttpMessageHandler(
        IAuthorizationHeaderValueProvider valueProvider)
    {
        _valueProvider = valueProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Authorization ??=
            await _valueProvider.GetAuthorizationHeaderValueAsync(cancellationToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
