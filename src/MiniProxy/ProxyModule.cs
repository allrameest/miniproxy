using EasyHttp.Http;
using MiniProxy.Caching;
using Nancy;

namespace MiniProxy
{
    public class ProxyModule : NancyModule
    {
        private readonly ICache _cache;

        public ProxyModule(ProxyConfiguration proxyConfiguration, ICache cache)
        {
            _cache = cache;

            foreach (var endPoint in proxyConfiguration.EndPoints)
            {
                SetupEndpoint(endPoint.Key, endPoint.Value);
            }

            if (proxyConfiguration.EnableCors)
            {
                After += ctx => ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            }
        }

        private void SetupEndpoint(string key, string baseUrl)
        {
            Get["/" + key + "/{url*}"] = x =>
            {
                var url = baseUrl + (string) x.url;

                var response = _cache.GetOrAdd(url, () => ToEndpointResponse(MakeRequest(url)));

                return Response.AsText(response.RawText, response.ContentType);
            };
        }

        private static HttpResponse MakeRequest(string url)
        {
            var client = new HttpClient();
            client.Request.Accept = HttpContentTypes.ApplicationXml;
            return client.Get(url);
        }

        private static EndpointResponse ToEndpointResponse(HttpResponse response)
        {
            return new EndpointResponse
            {
                RawText = response.RawText,
                ContentType = response.ContentType
            };
        }
    }
}