using Autofac;
using Autofac.Extensions.DependencyInjection;
using BabySleep.Common.Helpers;
using BabySleep.Common.Interfaces;
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

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    // requires using Microsoft.AspNetCore.Http;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //load base settings
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true) //load local settings
    .AddEnvironmentVariables();

builder.Services.AddOptions();
var section = builder.Configuration.GetSection(FirebaseOptions.Firebase);
builder.Services.Configure<FirebaseOptions>(section);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new DIContanerWebModule()));
builder.Services.AddTransient<IChildrenHelper, ChildrenHelper>();

//Create customer config
builder.Services.AddSingleton<ICustomerConfig, CustomerConfig>(scope =>
{
    CustomerConfig config = new CustomerConfig();

    config.SmtpEmail = builder.Configuration.GetValue<string>("Smtp:Email");
    config.SmtpPassword = builder.Configuration.GetValue<string>("Smtp:Password");

    config.FirebaseApiKey = builder.Configuration.GetValue<string>("Firebase:ApiKey");

    config.AwsAccessKey = builder.Configuration.GetValue<string>("Aws:AccessKey");
    config.AwsSecretKey = builder.Configuration.GetValue<string>("Aws:SecretKey");

    return config;
});

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

app.UseCookiePolicy();

app.Run();
