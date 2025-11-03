using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private readonly string _logFilePath;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);
    }

    public async Task InvokeAsync(HttpContext context)
    {

        //Request
        string requestBody = await ReadRequestBody(context.Request);
        var headersToLog = new[] { "Content-Type", "Host" };
        string requestHeaders = string.Join(", ", context.Request.Headers
            .Where(h => headersToLog.Contains(h.Key, StringComparer.OrdinalIgnoreCase))
            .Select(h => $"[{h.Key}: {h.Value}]"));

        string queryParams = string.Join(", ",
            context.Request.Query.Select(q => $"[{q.Key}, {q.Value}]"));

        string routeParams = string.Join(", ",
            context.Request.RouteValues.Select(rp => $"[{rp.Key}, {rp.Value}]"));

        string requestLog = $"Incoming Request: {context.Request.Method} {context.Request.Path}\n" +
                            $"Headers: {requestHeaders}\n" +
                            $"Route Params: {routeParams}\n" +
                            $"Query Params: {queryParams}\n" +
                            $"Body: {requestBody}\n";

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        //Response
        string responseBodyText = await ReadResponseBody(context.Response);
        string responseHeaders = string.Join(", ", context.Response.Headers
            .Select(h => $"[{h.Key}, {h.Value}]"));

        string responseLog = $"Outgoing Response: {context.Response.StatusCode}\n" +
                             $"Headers: {responseHeaders}\n" +
                             $"Body: {responseBodyText}\n" +
                             $"------------------------------------------------------------\n\n";

        string logEntry = requestLog + "\n" + responseLog;


        await WriteLogToFileAsync(logEntry);


        _logger.LogInformation(logEntry);

        // Copy response back
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }

    private async Task WriteLogToFileAsync(string text)
    {
        await File.AppendAllTextAsync(_logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - \n{text}");
    }
}