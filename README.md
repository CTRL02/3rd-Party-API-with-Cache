# 3rd-Party-API-with-Cache

## Project Overview
This project is a simple web API that calls a third-party API to fetch data. To improve performance, the results are cached using **Redis**, so subsequent requests are served from the cache instead of fetching fresh data from the external API each time.

By caching the API response, we minimize the number of external API calls, reducing latency and improving response times significantly, especially when data does not change frequently.

## Key Features
- Fetches data from a third-party API.
- Caches the results using Redis for faster subsequent requests.
- Displays the cached results, significantly improving performance compared to fetching data from the API every time.

## What is Redis?
**Redis** (Remote Dictionary Server) is an in-memory data structure store, used as a database, cache, and message broker. It supports various data structures like strings, hashes, lists, sets, and more.

In this project, Redis is used as a **cache** to store the API responses. The data is stored in memory, allowing for extremely fast read operations, thus reducing the time it takes to fetch the same data multiple times. 

### Redis Caching Benefits:
- **Improved Performance**: Data retrieval is faster because it's coming from memory instead of an external API.
- **Reduced API Calls**: By caching the results, you reduce the number of external API calls, saving bandwidth and potential costs.
- **Scalability**: Redis is highly scalable and can handle large amounts of data efficiently.
- **Security**: Redis can also be used to store info about each user like number of requests they made and then block some of them to mitigate against attacks like DOS attack.
## Performance Comparison

The following images demonstrate the performance difference when the API is called without caching and with caching.

### Without Cache:
![Without Cache](https://github.com/user-attachments/assets/4d6d582c-debe-43d7-8fd7-20210cc7112b)

### With Cache:
![With Cache](https://github.com/user-attachments/assets/104d0766-0dad-4f97-b663-98006efba516)


As shown, when the API is called without using cache, the response time is slower because it involves fetching data from the third-party API. In contrast, with caching enabled, the response time is significantly reduced, as the data is retrieved directly from Redis.

## How It Works

1. **First API Call**: The application calls the third-party API and stores the results in Redis.
2. **Subsequent API Calls**: The application checks Redis for cached results. If the data exists, it returns the cached data; if not, it fetches fresh data from the third-party API and stores it again in the cache.

## Setup Instructions

### Prerequisites
- .NET 6.0 or later
- Redis Cache (either locally id OS not Windows or using Docker)

### Running Redis Using Docker
```bash
docker run -d --name my-redis -p 6379:6379 redis
