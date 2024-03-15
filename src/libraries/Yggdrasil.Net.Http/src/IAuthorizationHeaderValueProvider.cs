namespace Yggdrasil.Net.Http;

public interface IAuthorizationHeaderValueProvider
{
    Task<AuthenticationHeaderValue?> GetAuthorizationHeaderValueAsync(CancellationToken cancellationToken);
}
