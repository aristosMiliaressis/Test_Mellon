using ApiClient.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient
{
    public class HttpHelper
    {
        public static async Task<HttpContent> SendRequestAsync(Uri fullUrl, HttpMethod method, string accessToken, dynamic content = null)
        {
            HttpResponseMessage response = null;
            using (var handler = new HttpClientHandler())
            {
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(method, fullUrl);

                    if (accessToken != null)
                        request.Headers.Add("Authorization", "Token " + accessToken);

                    if (content != null)
                    {
                        var jsonContent = JsonConvert.SerializeObject(content);

                        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    }

                    try
                    {
                        response = await client.SendAsync(request);
                    }
                    catch (Exception ex)
                    {
                        throw new TransferErrorException(ex);
                    }
                }
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ResourceNotFoundException(fullUrl.ToString());
            }

            if (response.StatusCode != HttpStatusCode.OK
             && response.StatusCode != HttpStatusCode.Created
             && response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new HttpErrorResponseException("HTTP error occurred!", response.StatusCode);
            }

            return response.Content;
        }
    }
}
