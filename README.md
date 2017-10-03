## Dot Net Core + Redis to create Distributed cache API.

#### Intro

When an application consumes an external data base, its performance can be bottlenecked depending on business rules, middle tier application archtecture and the DB management system. Adding to it, you must check how much time and how big is the result sent to your cliente.

You must take a look into your api respondes. Most of our web clients consume some informations that hardly change for every minutes. Looking into data size, change frequency and calls frequency, we must understand which API should be cached and for how long it should be.


#### Why To Cache Data

In plain words, cache is used when we want to client have access data that hardly change, avoiding going through all business and archtecture.

It's main purpouse is to reduce the time to access data stored outside the app memory. Normally, just the first request to some outside store is executed and the next will consumethe cache DB. 

This strategy improves the usage of external resources and app processing to get same data again and again.

##### Distributed Cache

##### Why Redis

#### How To

##### Installing Redis

> You can check how to install Redis from this tutoria on Youtube.
Check this video: https://www.youtube.com/watch?v=DYaFW5MhfG8


##### Creating .Net Core project

##### Library


