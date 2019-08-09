#if !NET45
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TXTextControl.ReportingCloud
{
    internal static class HttpContentExtension
    {
        internal static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
        }

        internal static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, object content, JsonMediaTypeFormatter formatter)
        {
            return client.PostAsync(url,
                new StringContent(JsonConvert.SerializeObject(content, formatter.SerializerSettings), Encoding.UTF8, "application/json"));
        }
    }

    internal class JsonMediaTypeFormatter
    {
        internal JsonSerializerSettings SerializerSettings { get; set; }
    }
}
#endif