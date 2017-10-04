## Dot Net Core + Redis to create Distributed cache API.

### Intro

When an application consumes an external data base, its performance can be bottlenecked depending on business rules, middle tier application archtecture and the DB management system. Adding to it, you must check how much time and how big is the result sent to your cliente.

You must take a look into your api respondes. Most of our web clients consume some informations that hardly change for every minutes.

Looking into data size, change frequency and calls frequency, we must understand which API should be cached and for how long it should be.


##### Caching Data

Cache's main purpouse is to reduce the time to access data stored outside the app memory. Normally, just the first request process all the business pipeline and go to some outside store to retrieve data, the next calls will consume same data from a cache DB. 

Using cache on app, you will improve the usage of external resources freeing this resources to other user/process. 

> In plain words, cache is used when we want to have access to data that hardly change, avoiding going through all business and archtecture.

##### Distributed Cache

Beside of basic cache that stores data at current servers memory, in distributed cache we share data to multiple servers and this cached data can be used to any app server that needs.

##### Installing Redis

> You can check how to install Redis from this tutoria on Youtube.
Check this video: https://www.youtube.com/watch?v=DYaFW5MhfG8


### Project Overview

##### Package Dependency

You will need to add thispackage to your project.
https://www.nuget.org/packages/Microsoft.Extensions.Caching.Redis/

##### API/Startup

At this point, you need to setup the Redis connection at ConfigureServices method on Startup.cs.

```cs
 services.AddDistributedRedisCache(o =>
            {
                o.Configuration = "127.0.0.1:6379";
            });
```
> I set the default port for Redis connection. Ypu may need to change it to your own connection.


