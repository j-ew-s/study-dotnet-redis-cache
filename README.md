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

This tuto objective does not cover the Redis install. You can check how to install Redis from this tutoria on Youtube.
[Check this tutorial on YouTube](https://www.youtube.com/watch?v=DYaFW5MhfG8)


### Project Overview

##### Package Dependency

You may need to add this package to your project.
[Microsoft.Extensions.Caching.Redis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Redis/)

##### API/Startup

At this point, you need to setup the Redis connection at ConfigureServices method on Startup.cs.

```cs
 services.AddDistributedRedisCache(o =>
            {
                o.Configuration = "127.0.0.1:6379";
            });
```
> I set the default port for Redis connection. Ypu may need to change it to your own connection.

##### API/Controller/BlogPostsController.cs

Here is the API that will have data cached.
First step is to set the IDistributedCache interface at controller's constructor
```cs
private readonly IDistributedCache _distributedCache;
public BlogPostsController(IDistributedCache distributedCache)
{
    _distributedCache = distributedCache;
}
```

