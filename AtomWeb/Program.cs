using AtomData.Interfaces;
using AtomData.Repositories;
using AtomServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Driver;
using System.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();


builder.Services.AddTransient<IMongoDbApiRepository, MongoDbApiRepository>();
builder.Services.AddTransient<MongoDbApiServices>();
builder.Services.AddTransient<IDiscordBotApiRepository, DiscordBotApiRepository>();
builder.Services.AddTransient<DiscordBotApiServices>();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
  /*  app.UseStatusCodePagesWithRedirects("/Home/StatusCodeErrorPage?statusCode={0}");*/
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
