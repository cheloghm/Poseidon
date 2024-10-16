﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Poseidon.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly int _limit = 1000; // e.g., 100 requests
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        // Updated to async
        public async Task InvokeAsync(HttpContext context)
        {
            // Bypass rate limiting for health checks
            if (context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            if (clientIp != null)
            {
                var key = $"RateLimit_{clientIp}";
                var requestCount = _cache.GetOrCreate<int>(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _timeWindow;
                    return 0;
                });

                if (requestCount >= _limit)
                {
                    context.Response.StatusCode = 429; // Too Many Requests
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }

                _cache.Set(key, requestCount + 1);
            }

            await _next(context);  // Awaiting next middleware
        }
    }

}
