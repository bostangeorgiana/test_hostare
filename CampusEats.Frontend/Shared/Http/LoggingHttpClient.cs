namespace CampusEats.Frontend.Shared.Http;

public class LoggingHttpClient
{
    public void LogRequest(string method, string url, object body)
    {
        Console.WriteLine($"[HTTP] {method} → {url}");
        Console.WriteLine($"Payload: {System.Text.Json.JsonSerializer.Serialize(body)}");
    }

    public void LogResponse(HttpResponseMessage response)
    {
        Console.WriteLine($"Response: {(int)response.StatusCode} {response.ReasonPhrase}");
    }
}