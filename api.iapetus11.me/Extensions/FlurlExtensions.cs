using System.Reflection;
using System.Text.Json.Serialization;
using api.iapetus11.me.Models;
using Flurl.Http;

namespace api.iapetus11.me.Extensions;

public static class FlurlExtensions
{
    public static IFlurlRequest SetShieldQueryParams(this IFlurlRequest request, ShieldQueryParams shieldParams)
    {
        var properties = shieldParams.GetType().GetProperties();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(shieldParams);

            if (value is null) continue;
            
            request = request.SetQueryParam(
                prop.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name, value);
        }

        return request;
    }
}