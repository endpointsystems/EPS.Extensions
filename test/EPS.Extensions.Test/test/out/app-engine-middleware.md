Title: Build Middleware to replicate AppEngine Metadata
Description: "Building middleware for ASP.NET Core is very easy - if you know where it does, and how to implement it. So building some simple test middleware to mock up App Engine headers is a piece of cake."
Published: 10/3/2019
Updated: 05/07/2020
Keywords: AppEngine, ASP.NET Core, middleware, request headers
Image: https://res.cloudinary.com/endpoint-systems/image/upload/v1548616596/gae_qlaf4t.png
Categories: 
    - AppEngine
---

One of the best features of using Google Cloud's AppEngine with web apps is that AppEngine provides geolocation info for users in the [request headers](https://cloud.google.com/appengine/docs/flexible/dotnet/reference/request-headers). The list isn't nearly as long as the list you get in [AppEngine Standard](https://cloud.google.com/appengine/docs/standard/go/reference/request-response-headers), but you still get the important ones:

- X-AppEngine-Country
- X-AppEngine-Region
- X-AppEngine-City
- X-AppEngine-CityLatLong
- X-Cloud-Trace-Context (for identifying the current request)

When building and debugging ASP.NET Core web apps for AppEngine, however, you don't get these things from Kestrel or anywhere else - so to replicate them, you have to inject them through middleware. 

[ASP.NET Core middleware documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.0) shows you can easily build middleware for what we're trying to do in numerous places - you can create a delegate in your `Startup` class, for example. What we did instead was build a separate class, appropriately titled `AppEngineMiddleware`, which we use to inject our headers into the requests. Here's how we built ours:

```
public class AppEngineMiddleware
{
    protected readonly IAppCache cache;
    protected readonly RequestDelegate next;
    protected readonly GcpSettings gcpSettings;

    public AppEngineMiddleware(IAppCache appCache, 
        IOptions<GcpSettings> settings, RequestDelegate requestDelegate)
    {
        cache = appCache;
        next = requestDelegate;
        gcpSettings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        var country = cache.GetOrAdd("request.country", () => gcpSettings.AppEngineCountry);
        var region = cache.GetOrAdd("request.region",  () => gcpSettings.AppEngineRegion);
        var city = cache.GetOrAdd("request.city",  () => gcpSettings.AppEngineCity);
        var geo = cache.GetOrAdd("request.geo", () => gcpSettings.AppEngineCityLatLong.Split(","));

        var trace = !context.User.Identity.IsAuthenticated
            ? context.TraceIdentifier
            : context.User.FindFirst(Startup.NAME_IDENTIFIER).Value;

        context.Request.Headers.Add("X-Cloud-Trace-Context", trace);
        context.Request.Headers.Add("X-AppEngine-Country", country);
        context.Request.Headers.Add("X-AppEngine-Region",region);
        context.Request.Headers.Add("X-AppEngine-City", city);
        context.Request.Headers.Add("X-AppEngine-CityLatLong", geo);
        await next(context);
    }

}
```

It's fairly self-explanatory - we're reading our country, region, city & coordinates out of our `GcpSettings` configuration object and putting them in the request headers so that they'll always show up; we also store those values in our LazyCache for performance considerations - or if we want to dynamically change those values. 

We have our middleware registered in our Startup class so it only runs during development:

```
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseForwardedHeaders();
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseMiddleware<AppEngineMiddleware>();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
```

Keep in mind that the middleware is executed on *every* request, so you want to make sure you're efficient in there, or you'll really bog down. This isn't a good place for printing Debug statements, for example. 

AppEngine is a great platform for ASP.NET Core development, and the value added by having geolocation functionality makes for a great developer experience. 

*Looking to integrate your legacy or existing web application with the cloud and other resources? [Contact us](/contact) and find out how you can save more money using our resources over commercial competitors!*
