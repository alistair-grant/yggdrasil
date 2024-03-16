using Microsoft.Extensions.Primitives;

namespace Yggdrasil.AspNetCore.Http;

public class PassThroughAuthorizationHeaderValueProviderTest
{
    public static TheoryData<AuthenticationHeaderValue?, StringValues> ExpectedValues
    {
        get
        {
            var scheme = Guid.NewGuid().ToString();
            var parameter = Guid.NewGuid().ToString();

            var result = new TheoryData<AuthenticationHeaderValue?, StringValues>
            {
                { null, new StringValues() },
                { new AuthenticationHeaderValue(scheme), new StringValues($"{scheme}") },
                { new AuthenticationHeaderValue(scheme, parameter), new StringValues($"{scheme} {parameter}") }
            };

            return result;
        }
    }

    [Theory]
    [MemberData(nameof(ExpectedValues))]
    public async Task GetAuthorizationHeaderValueAsync_HttpContextAvailable_ReturnsExpectedValue(
        AuthenticationHeaderValue? expected, StringValues requestHeaderValue)
    {
        var requestHeaders = Substitute.For<IHeaderDictionary>();
        requestHeaders.Authorization.Returns(requestHeaderValue);

        var request = Substitute.For<HttpRequest>();
        request.Headers.Returns(requestHeaders);

        var context = Substitute.For<HttpContext>();
        context.Request.Returns(request);

        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(context);

        var provider = new PassThroughAuthorizationHeaderValueProvider(contextAccessor);

        var actual = await provider.GetAuthorizationHeaderValueAsync(CancellationToken.None);

        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public async Task GetAuthorizationHeaderValueAsync_NoActiveHttpContext_ReturnsNull()
    {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();

        var provider = new PassThroughAuthorizationHeaderValueProvider(contextAccessor);

        var actual = await provider.GetAuthorizationHeaderValueAsync(CancellationToken.None);

        Assert.Null(actual);
    }
}
