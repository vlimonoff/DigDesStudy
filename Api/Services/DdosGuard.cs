using System.Collections.Concurrent;

namespace Api.Services
{
    public class TooManyRequestException : Exception
    {
        
    }

    public class DdosGuard
    {
        public ConcurrentDictionary<string, int> RequestControlDict { get; set; } = new ConcurrentDictionary<string, int>();

        private object _lock = new object();

        public void CheckRequest(string? token)
        {
            if (token == null)
                return;

            var dtn = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            var key = $"{token}_{dtn}";

            if (RequestControlDict.ContainsKey(key))
            {
                lock(_lock)
                {
                    var requests = RequestControlDict.TryGetValue(token, out var request) ? request : 0;
                    if (requests > 10)
                    {
                        throw new TooManyRequestException();
                    }
                    RequestControlDict.TryUpdate(token, requests, ++requests);
                }
            }
            RequestControlDict.TryAdd(token, 0);
        }
    }
}
