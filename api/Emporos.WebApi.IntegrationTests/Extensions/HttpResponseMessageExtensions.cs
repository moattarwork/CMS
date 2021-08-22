using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Emporos.WebApi.IntegrationTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ValidateAndReadContentAsync<T>(this HttpResponseMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            message.EnsureSuccessStatusCode();

            var content = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }        
        
        public static async Task<string> ReadValidationErrorContentAsync(this HttpResponseMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            return await message.Content.ReadAsStringAsync();
        }
    }
}