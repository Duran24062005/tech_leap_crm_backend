using Microsoft.AspNetCore.Http;
using TechLeap.Crm.BuildingBlocks.Web;
using Xunit;

namespace TechLeap.Crm.UnitTests;

public sealed class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task UsesIncomingCorrelationIdAndReturnsItInResponse()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "request-correlation";
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(context);

        Assert.Equal("request-correlation", context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString());
    }

    [Fact]
    public async Task CreatesCorrelationIdWhenHeaderIsMissing()
    {
        var context = new DefaultHttpContext();
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(context);

        var correlationId = context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString();
        Assert.False(string.IsNullOrWhiteSpace(correlationId));
        Assert.Equal(32, correlationId.Length);
    }
}
