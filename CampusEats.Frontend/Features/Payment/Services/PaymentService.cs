using System.Net.Http.Json;
using CampusEats.Frontend.Features.Payment.Models;

namespace CampusEats.Frontend.Features.Payment.Services;

public class PaymentService(HttpClient http)
{
    public async Task<PaymentResponse> PayAsync(PaymentRequest request)
    {
        var res = await http.PostAsJsonAsync("/payments/process", request);

        PaymentResponse? body = null;

        try
        {
            body = await res.Content.ReadFromJsonAsync<PaymentResponse>();
        }
        catch
        {
            // backend might return HTML or plain text
        }

        if (body != null)
            return body;
        
        return new PaymentResponse
        {
            Success = false,
            Message = "Payment failed — server returned an invalid response.",
            Amount = 0
        };
    }
}