using Autofac;
using Autofac.Extensions.DependencyInjection;
using BabySleep.Core;
using BabySleepWeb.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();//.AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/Login/Login";
    });

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


builder.Services.AddOptions();
var section = builder.Configuration.GetSection(FirebaseOptions.Firebase);
builder.Services.Configure<FirebaseOptions>(section);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new DIContanerWebModule()));
builder.Services.AddTransient<IChildrenHelper, ChildrenHelper>();

//builder.Logging.AddSerilog(logger);

builder.Host.UseSerilog(
    (ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

//var loggerFactory = app.Services.GetService<ILoggerFactory>();
//loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sleep}/{action=Index}/{id?}");

app.Run();
