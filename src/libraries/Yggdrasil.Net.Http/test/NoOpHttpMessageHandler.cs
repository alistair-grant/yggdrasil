namespace Yggdrasil.Net.Http;

public class NoOpHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResponseMessage(HttpStatusCode.NoContent);
        return Task.FromResult(result);
    }
}
