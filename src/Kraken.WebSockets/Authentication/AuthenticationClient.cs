using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.WebSockets.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthenticationClient : IAuthenticationClient
    {
        private readonly string uri;
        private readonly SecureString apiKey;
        private readonly SecureString apiSecret;
        private readonly int version;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="apiSecret">The API secret.</param>
        public AuthenticationClient(string uri, SecureString apiKey, SecureString apiSecret, int version = 0)
        {
            this.uri = uri;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
            this.version = version;
        }


        public async Task<AuthToken> GetWebsocketToken()
        {
            return (await QueryPrivate("GetWebSocketsToken")).Value<AuthToken>();
        }

        private async Task<JObject> QueryPrivate(string method, string props = null)
        {
            // generate a 64 bit nonce using a timestamp at tick resolution
            var nonce = DateTime.Now.Ticks;
            props = "nonce=" + nonce + props;


            var path = $"/{version}/private/{method}";
            var address = $"{uri}{path}";

            var request = new HttpRequestMessage(HttpMethod.Post, address)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string,string>
                {
                    {"nonce", nonce.ToString() }
                }),
                Headers =
                {
                    {"API-Key", apiKey.ToPlainString()},
                    {"API-Sign", Convert.ToBase64String(CalculateSignature(props, nonce, path)) }
                }
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request);
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var jsonReader = new JsonTextReader(new StreamReader(stream)))
                    {
                        var jObject = (JObject)new JsonSerializer().Deserialize(jsonReader);
                        return jObject;
                    }
                }
            }
        }

        private byte[] CalculateSignature(string props, long nonce, string path)
        {
            byte[] base64DecodedSecret = Convert.FromBase64String(apiSecret.ToPlainString());

            var np = nonce + Convert.ToChar(0) + props;

            var pathBytes = Encoding.UTF8.GetBytes(path);
            var hash256Bytes = SHA256Hash(np);
            var z = new byte[pathBytes.Count() + hash256Bytes.Count()];
            pathBytes.CopyTo(z, 0);
            hash256Bytes.CopyTo(z, pathBytes.Count());

            var signature = getHash(base64DecodedSecret, z);
            return signature;
        }

        private byte[] SHA256Hash(string value)
        {
            using (var hash = SHA256.Create())
            {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        private byte[] getHash(byte[] keyByte, byte[] messageBytes)
        {
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {

                Byte[] result = hmacsha512.ComputeHash(messageBytes);

                return result;

            }
        }
    }
}
