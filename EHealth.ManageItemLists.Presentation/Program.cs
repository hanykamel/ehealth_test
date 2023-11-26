using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EHealth.ManageItemLists.Application.Excel.Operations;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories.Lookups;
using EHealth.ManageItemLists.Presentation.Authentication;
using EHealth.ManageItemLists.Presentation.CustomMiddlewares;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using EHealth.ManageItemLists.Presentation.Identity;
using EHealth.ManageItemLists.Presentation.Keycloack;
using EHealth.ManageItemLists.Presentation.ServiceCollectionExtentions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureServices()
    .AddDataAccessServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExceptionHandlerMiddleware>();
builder.Services.AddScoped<UserPrivilegesMiddleware>();

//Fluient Validation
builder.Services.AddScoped<IValidationEngine, ValidationEngine>();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("eg-EG");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("eg-EG") };
    options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
    {
        return Task.FromResult(new ProviderCultureResult("en-US"));
    }));
});


//Authentication 
var keycloackConfig = builder.Configuration.GetSection("KeycloackConfig").Get<KeycloackConfig>();
builder.Services.ConfigureJWTWithKeycloack(builder.Environment.IsDevelopment(), keycloackConfig);
builder.Services.AddSingleton(keycloackConfig);
builder.Services.TryAddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.TryAddSingleton<IClaimsTransformation, NoopClaimsTransformation>();
builder.Services.TryAddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
builder.Services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EHealth.ManageItemLists", Version = "v1" });
    //First we define the security scheme
    c.AddSecurityDefinition("Bearer", //Name the security scheme
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
            Scheme = JwtBearerDefaults.AuthenticationScheme //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
});
builder.Services.AddScoped(typeof(IIdentityProvider), typeof(IdentityProvider));
builder.Services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("EHealth.ManageItemLists.Application")));



var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin 
    .AllowCredentials());

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<UserPrivilegesMiddleware>();
app.UseAuthorization();

app.MapControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();
