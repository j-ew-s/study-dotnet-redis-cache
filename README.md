## Dot Net Core + Redis to create Distributed cache API.

### Intro

Make our App faster enough is a big deal today, and some approaches can help us to improve this time to response better.

Consuming data from an external DB somethimes can make the API response a little bit more expensive. In some cases the data you want to are trivial and it hardly changes. In some cases, the business rules, middle tier archtecture and DB can bottleneck your reseponse, and what should be a fast and easy get, take too long.

When you face this kind of situation, you must take a look into your api responses, looking into data size, data change frequency and calls frequency. Depending on the sitation, cache this response is a good approache, taking in consideration, for how long this cache must be keeped. 


##### Caching Data

Cache's main purpouse is to reduce the time to access data stored outside the app memory. Normally, just the first request process all the business pipeline and go to some outside store to retrieve data, the next calls will consume same data from a cache DB. 

Using cache on app, you will improve the usage of external resources freeing this resources to other user/process. 

> In plain words, cache is used when we want to have access to data that hardly change, avoiding going through all business and archtecture.

##### Distributed Cache

Beside of basic cache that stores data at current servers memory, in distributed cache we share data to multiple servers and this cached data can be used to any app server that needs.

##### Installing Redis

This tuto objective does not cover the Redis install. [You can check how to install Redis from this tutorial on Youtube.](https://www.youtube.com/watch?v=DYaFW5MhfG8)


### Project Overview

##### Package Dependency

You may need to add this package to your project.
[Microsoft.Extensions.Caching.Redis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Redis/)

##### API/Startup

At this point, you need to setup the Redis connection at ConfigureServices method on Startup.cs:

```cs
 services.AddDistributedRedisCache(o =>
            {
                o.Configuration = "127.0.0.1:6379";
            });
```
> I set the default port for Redis connection. You may need to change it to your own connection.

##### API/Controller/BlogPostsController.cs

Here is the API that will have data cached.

First step is to set the IDistributedCache interface at controller's constructor:
```cs
private readonly IDistributedCache _distributedCache;
public BlogPostsController(IDistributedCache distributedCache)
{
    _distributedCache = distributedCache;
}
```

We will use cache on the HTTP verb Get:

```cs
[HttpGet]
 public async Task<string>  Get()
{
     var watch = System.Diagnostics.Stopwatch.StartNew();
     var cachedData = await _distributedCache.GetStringAsync(_cacheKey);

     if (string.IsNullOrEmpty(cachedData))
     {
         cachedData = await ApiCaller.GetPost();
         await _distributedCache.SetStringAsync(_cacheKey, cachedData);
     }
     else
     {
         watch.Stop();
         return "CACHED. You took " + watch.ElapsedMilliseconds + " Milliseconds to have your response. Post  : " + cachedData;
     }
     watch.Stop();
     return "NO CACHED data. You took " + watch.ElapsedMilliseconds + " Milliseconds to have your response. The result from API (Post): " + cachedData
 }
```

Note that I'll need a key to define our data on Redis. Lets add it to our Controller:

```cs
private readonly string _cacheKey = "BlogPost";
```

To control how much time data will be cached, you should add  _distributedCache variable declaration the following line:

```cs
 private readonly DistributedCacheEntryOptions _cacheOptions;
```
then set it on constructor to the appropriated value. For this tutorial, I am setting it to 1 minute :
```cs
 _cacheOptions = new DistributedCacheEntryOptions()
{
    AbsoluteExpirationRelativeToNow = (DateTime.Now.AddMinutes(1) - DateTime.Now)
};
```
... and then, add this option to SetStringAsync :
```cs
  await _distributedCache.SetStringAsync(_cacheKey, cachedData, _cacheOptions);
```

### Results

You must run the project and go to : <localhost>/api/BlogPosts

##### First Call or Call After Expired Cache Time 

When you run it for the very first time or your cache time expired, you get this message : 
> NO CACHED data. You took 3088 Milliseconds to have your response. The result from API (Post): ...

Please, note that we are calling https://jsonplaceholder.typicode.com/posts to get our data. It is just to mock data, and you can check how long it took.

##### Other Calls

When you call the BlogPosts again, you will receive a result like this: 
> CACHED. You took 1 Milliseconds to have your response. Post  : ...

Note that your reponse time is pettry much nicer. From 3088 we jumped to 1 Millisecond. I think using cached data is something that can improve your app :D
