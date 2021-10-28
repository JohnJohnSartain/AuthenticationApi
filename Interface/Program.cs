using AutoWrapper;
using Http;
using Interface.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SartainStudios.Token;
using Services;

var builder = WebApplication.CreateBuilder(args);

const string ApplicationVersion = "1.0.0";
string ApplicationName = builder.Configuration["ApplicationInformation:ApplicationName"];
int AuthenticationExpirationInMinutes = int.Parse(builder.Configuration["Authentication:ExpirationInMinutes"]);
string AuthenticationSecret = builder.Configuration["Authentication:Secret"];
string CorsPolicyName = "CorsOpenPolicy";

#region Filters
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddMvc(options => { options.Filters.Add(typeof(ValidateModelStateAttribute)); });

builder.Services.AddMvc(options => { options.Filters.Add(typeof(UnauthorizedAccessExceptionHandlerAttribute)); });
#endregion

// Add cors
builder.Services.AddCors(o => o.AddPolicy(CorsPolicyName, builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

#region services
builder.Services.AddSingleton(typeof(IAutoWrapperHttp<>), typeof(AutoWrapperHttp<>));

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

builder.Services.AddSingleton<IToken>(new JwtToken(AuthenticationSecret, AuthenticationExpirationInMinutes));
#endregion

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(ApplicationVersion,
                    new OpenApiInfo { Title = ApplicationName, Version = ApplicationVersion });
});

var app = builder.Build();

app.UseCors(CorsPolicyName);

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{ApplicationVersion}/swagger.json",
                          $"{ApplicationName} {ApplicationVersion}"));
}

app.UseHttpsRedirection();

app.UseApiResponseAndExceptionWrapper();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();