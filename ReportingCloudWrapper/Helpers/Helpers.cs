using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
#if NET45
using System.Net.Http.Formatting;
#endif
using System.Net.Http.Headers;
using System.Text;

namespace TXTextControl.ReportingCloud
{
    public static class Helpers
    {
        internal static JsonMediaTypeFormatter TypeFormatter()
        {
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // add the CamelCasePropertyNamesContractResolver to convert
            // the .NET parameter to JSON camelCase formatting
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return formatter;
        }

        /*-------------------------------------------------------------------------------------------------------
        // ** CreateHttpClient **
        // This factory method creates and returns a HttpClient object with the
        // proper headers, the base URL and the authorization
        *-----------------------------------------------------------------------------------------------------*/
        internal static HttpClient CreateHttpClient(Uri WebApiBaseUrl, string Username, string Password, string APIKey)
        {
            Uri m_sWebApiBaseUrl = WebApiBaseUrl;
            
            HttpClient client = new HttpClient();

            client.BaseAddress = m_sWebApiBaseUrl;
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            if (APIKey != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "ReportingCloud-APIKey " + APIKey);
            }
            else
            {
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodeTo64(Username + ":" + Password));
            }

            return client;
        }

        /*-------------------------------------------------------------------------------------------------------
        // ** EncodeTo64 **
        // This static method encodes a string to Base64
        *-----------------------------------------------------------------------------------------------------*/
        /// <summary>
        /// This method encodes a string to a Base64 string.
        /// </summary>
        /// <param name="toEncode">The string to encode.</param>
        /// <returns></returns>
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
