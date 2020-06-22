Title: Build Middleware to replicate AppEngine Metadata
Description: "Building middleware for ASP.NET Core is very easy - if you know where it does, and how to implement it. So building some simple test middleware to mock up App Engine headers is a piece of cake."
Published: 10/3/2019
Updated: 05/07/2020
Keywords: AppEngine, ASP.NET Core, middleware, request headers
Image: https://res.cloudinary.com/endpoint-systems/image/upload/v1548616596/gae_qlaf4t.png
Categories: 
    - AppEngine
---

<p>One of the best features of using Google Cloud's AppEngine with web apps is that AppEngine provides geolocation info for users in the <a href="https://cloud.google.com/appengine/docs/flexible/dotnet/reference/request-headers">request headers</a>. The list isn't nearly as long as the list you get in <a href="https://cloud.google.com/appengine/docs/standard/go/reference/request-response-headers">AppEngine Standard</a>, but you still get the important ones:</p>
<ul>
<li>X-AppEngine-Country</li>
<li>X-AppEngine-Region</li>
<li>X-AppEngine-City</li>
<li>X-AppEngine-CityLatLong</li>
<li>X-Cloud-Trace-Context (for identifying the current request)</li>
</ul>
<p>When building and debugging ASP.NET Core web apps for AppEngine, however, you don't get these things from Kestrel or anywhere else - so to replicate them, you have to inject them through middleware.</p>
<p><a href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.0">ASP.NET Core middleware documentation</a> shows you can easily build middleware for what we're trying to do in numerous places - you can create a delegate in your <code>Startup</code> class, for example. What we did instead was build a separate class, appropriately titled <code>AppEngineMiddleware</code>, which we use to inject our headers into the requests. Here's how we built ours:</p>
<pre><code>public class AppEngineMiddleware
{
    protected readonly IAppCache cache;
    protected readonly RequestDelegate next;
    protected readonly GcpSettings gcpSettings;

    public AppEngineMiddleware(IAppCache appCache, 
        IOptions&lt;GcpSettings&gt; settings, RequestDelegate requestDelegate)
    {
        cache = appCache;
        next = requestDelegate;
        gcpSettings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        var country = cache.GetOrAdd(&quot;request.country&quot;, () =&gt; gcpSettings.AppEngineCountry);
        var region = cache.GetOrAdd(&quot;request.region&quot;,  () =&gt; gcpSettings.AppEngineRegion);
        var city = cache.GetOrAdd(&quot;request.city&quot;,  () =&gt; gcpSettings.AppEngineCity);
        var geo = cache.GetOrAdd(&quot;request.geo&quot;, () =&gt; gcpSettings.AppEngineCityLatLong.Split(&quot;,&quot;));

        var trace = !context.User.Identity.IsAuthenticated
            ? context.TraceIdentifier
            : context.User.FindFirst(Startup.NAME_IDENTIFIER).Value;

        context.Request.Headers.Add(&quot;X-Cloud-Trace-Context&quot;, trace);
        context.Request.Headers.Add(&quot;X-AppEngine-Country&quot;, country);
        context.Request.Headers.Add(&quot;X-AppEngine-Region&quot;,region);
        context.Request.Headers.Add(&quot;X-AppEngine-City&quot;, city);
        context.Request.Headers.Add(&quot;X-AppEngine-CityLatLong&quot;, geo);
        await next(context);
    }

}
</code></pre>
<p>It's fairly self-explanatory - we're reading our country, region, city &amp; coordinates out of our <code>GcpSettings</code> configuration object and putting them in the request headers so that they'll always show up; we also store those values in our LazyCache for performance considerations - or if we want to dynamically change those values.</p>
<p>We have our middleware registered in our Startup class so it only runs during development:</p>
<pre><code>public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseForwardedHeaders();
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseMiddleware&lt;AppEngineMiddleware&gt;();
    }
    else
    {
        app.UseExceptionHandler(&quot;/Error&quot;);
        app.UseHsts();
    }
</code></pre>
<p>Keep in mind that the middleware is executed on <em>every</em> request, so you want to make sure you're efficient in there, or you'll really bog down. This isn't a good place for printing Debug statements, for example.</p>
<p>AppEngine is a great platform for ASP.NET Core development, and the value added by having geolocation functionality makes for a great developer experience.</p>
<p><em>Looking to integrate your legacy or existing web application with the cloud and other resources? <a href="/contact">Contact us</a> and find out how you can save more money using our resources over commercial competitors!</em></p>
