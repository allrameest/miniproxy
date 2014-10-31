using System.Collections.Generic;

namespace MiniProxy
{
    public class ProxyConfiguration
    {
        public bool EnableCors { get; set; }
        public IDictionary<string, string> EndPoints { get; set; }
    }
}