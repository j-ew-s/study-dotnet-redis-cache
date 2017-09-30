using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedCacheRedis.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedCacheRedis.Api.Controllers
{
    [Route("api/[controller]")]
    public class BlogPostsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        private readonly string _cacheKey = "BlogPost";
            

        public BlogPostsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

            _cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = (DateTime.Now.AddMinutes(1) - DateTime.Now)
            };

        }
        // GET: api/values
        [HttpGet]
        public async Task<string>  Get()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var cachedData = await _distributedCache.GetStringAsync(_cacheKey);

            if (string.IsNullOrEmpty(cachedData))
            {
                await _distributedCache.SetStringAsync(_cacheKey, cachedData, _cacheOptions);
            }
            else
            {
                watch.Stop();
                return "CACHED. You took " + watch.ElapsedMilliseconds + " Milliseconds to have your response. Post  : " + cachedData;
            }
            watch.Stop();
            return "NO CACHED data. You took " + watch.ElapsedMilliseconds + " Milliseconds to have your response. The result from API (Post): " + cachedData;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
