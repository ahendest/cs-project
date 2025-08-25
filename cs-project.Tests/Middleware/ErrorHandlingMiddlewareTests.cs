using cs_project.Middleware;
using cs_project.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace cs_project.Tests.Middleware;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task Invoke_SuccessRequest_PassesThrough()
    {
        var logger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var env = new Mock<IHostEnvironment>();
        env.SetupGet(e => e.EnvironmentName).Returns(Environments.Production);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.TraceIdentifier = "trace";

        var middleware = new ErrorHandlingMiddleware(_ => Task.CompletedTask, logger.Object, env.Object);

        await middleware.Invoke(context);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.False(context.Response.Headers.ContainsKey("X-Correlation-ID"));
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Equal(string.Empty, body);
        logger.Verify(l => l.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Never);
    }

    [Fact]
    public async Task Invoke_NotFoundException_Returns404AndLogs()
    {
        var logger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var env = new Mock<IHostEnvironment>();
        env.SetupGet(e => e.EnvironmentName).Returns(Environments.Production);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.TraceIdentifier = "corr-id";
        var exception = new NotFoundException("missing");
        RequestDelegate next = _ => throw exception;
        var middleware = new ErrorHandlingMiddleware(next, logger.Object, env.Object);

        await middleware.Invoke(context);

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Equal("corr-id", context.Response.Headers["X-Correlation-ID"].ToString());
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        Assert.Equal(404, root.GetProperty("status").GetInt32());
        Assert.Equal("NotFoundException", root.GetProperty("title").GetString());
        Assert.Equal("missing", root.GetProperty("detail").GetString());
        Assert.Equal("corr-id", root.GetProperty("correlationId").GetString());
        logger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o,t) => true),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task Invoke_Exception_Development_ReturnsStackTrace()
    {
        var logger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var env = new Mock<IHostEnvironment>();
        env.SetupGet(e => e.EnvironmentName).Returns(Environments.Development);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.TraceIdentifier = "dev-id";
        var exception = new Exception("boom");
        RequestDelegate next = _ => throw exception;
        var middleware = new ErrorHandlingMiddleware(next, logger.Object, env.Object);

        await middleware.Invoke(context);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("dev-id", context.Response.Headers["X-Correlation-ID"].ToString());
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        Assert.Equal(500, root.GetProperty("status").GetInt32());
        Assert.Equal("Exception", root.GetProperty("title").GetString());
        Assert.Equal("boom", root.GetProperty("detail").GetString());
        Assert.True(root.TryGetProperty("stackTrace", out var trace) && !string.IsNullOrEmpty(trace.GetString()));
        logger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o,t) => true),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task Invoke_Exception_Production_ReturnsGenericMessage()
    {
        var logger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var env = new Mock<IHostEnvironment>();
        env.SetupGet(e => e.EnvironmentName).Returns(Environments.Production);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.TraceIdentifier = "prod-id";
        var exception = new Exception("boom");
        RequestDelegate next = _ => throw exception;
        var middleware = new ErrorHandlingMiddleware(next, logger.Object, env.Object);

        await middleware.Invoke(context);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("prod-id", context.Response.Headers["X-Correlation-ID"].ToString());
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        Assert.Equal("An unexpected error occurred.", root.GetProperty("detail").GetString());
        Assert.False(root.TryGetProperty("stackTrace", out _));
        logger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o,t) => true),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
