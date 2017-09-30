using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DistributedCacheRedis.Api.Services
{
    public static class ApiCaller
    {
        public static async Task<string> GetPost()
        {
            var http = new HttpClient();
            var url = String.Format("https://jsonplaceholder.typicode.com/posts");
            var response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
