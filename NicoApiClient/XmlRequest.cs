using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NicoApi {

    internal static class XmlRequest {

        private static T GetXmlResponse<T>(Stream stream) {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        private static async Task<T> GetStreamAsync<T>(string url, Func<Stream, T> func, int timeoutSec = 10000, string userAgent = "") {

            var uri = new Uri(url);

            using (var client = new HttpClient()) {

                if (string.IsNullOrEmpty(userAgent)) {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VocaDB", "1.0"));
                } else {
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                }
                client.Timeout = TimeSpan.FromSeconds(timeoutSec);

                using (var response = await client.GetAsync(uri)) {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    return func(stream);
                }

            }

        }

        /// <summary>
        /// Performers a HTTP request that returns XML and deserializes that XML result into object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="url">URL to be requested.</param>
        /// <returns>Deserialized object. Cannot be null.</returns>
        /// <exception cref="HttpRequestException">If the request failed.</exception>
        public static Task<T> GetXmlObjectAsync<T>(string url) => GetStreamAsync(url, stream => GetXmlResponse<T>(stream));

    }

}
