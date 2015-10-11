using System.Collections.Generic;

namespace MiniProxy
{
    public class ProxyConfiguration
    {
        public bool EnableCors { get; set; }
        public bool EnableCaching { get; set; }
        public IDictionary<string, string> EndPoints { get; set; }
    }
}