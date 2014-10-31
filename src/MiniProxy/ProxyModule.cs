using EasyHttp.Http;
using Nancy;

namespace MiniProxy
{
    public class ProxyModule : NancyModule
    {
        public ProxyModule(ProxyConfiguration proxyConfiguration)
        {
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
                    var client = new HttpClient();
                    client.Request.Accept = HttpContentTypes.ApplicationJson;
                    HttpResponse response = client.Get(baseUrl + x.url);

                    return Response.AsText(response.RawText, response.ContentType);
                };
        }
    }
}