namespace Yggdrasil.Net.Http;

public class AuthorizationHeaderHttpMessageHandlerTest
{
    public static TheoryData<AuthenticationHeaderValue?> ExpectedValues =>
        new()
        {
            new AuthenticationHeaderValue(RandomScheme()),
            null
        };

    [Theory]
    [MemberData(nameof(ExpectedValues))]
    public async Task SendAsync_RequestDoesNotHaveAuthorizationHeader_AddsExpectedHeader(
        AuthenticationHeaderValue? expected)
    {
        var request = new HttpRequestMessage();

        var valueProvider = Substitute.For<IAuthorizationHeaderValueProvider>();
        valueProvider.GetAuthorizationHeaderValueAsync(Arg.Any<CancellationToken>())
            .Returns(expected);

        await new HandlerWrapper(valueProvider)
            .SendAsync(request, CancellationToken.None);

        var actual = request.Headers.Authorization;

        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task SendAsync_RequestHasAuthorizationHeader_UsesExistingHeader()
    {
        var expected = new AuthenticationHeaderValue(RandomScheme());

        var request = new HttpRequestMessage();
        request.Headers.Authorization = expected;

        var valueProvider = Substitute.For<IAuthorizationHeaderValueProvider>();
        valueProvider.GetAuthorizationHeaderValueAsync(Arg.Any<CancellationToken>())
            .Returns(new AuthenticationHeaderValue(RandomScheme()));

        await new HandlerWrapper(valueProvider)
            .SendAsync(request, CancellationToken.None);

        var actual = request.Headers.Authorization;

        Assert.Same(expected, actual);
    }

    private static string RandomScheme() =>
        Guid.NewGuid().ToString();

    private class HandlerWrapper : AuthorizationHeaderHttpMessageHandler
    {
        public HandlerWrapper(IAuthorizationHeaderValueProvider valueProvider)
            : base(valueProvider)
        {
            InnerHandler = new NoOpHttpMessageHandler();
        }

        public new Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            base.SendAsync(request, cancellationToken);
    }
}
