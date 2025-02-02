using LoanCRUD.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;

var builder = WebApplication.CreateBuilder(args);

// Retrieve Google credentials path from configuration
var googleCredentialsPath = builder.Configuration["GoogleCredentialsPath"];
if (!string.IsNullOrEmpty(googleCredentialsPath))
{
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", googleCredentialsPath);

    if (FirebaseApp.DefaultInstance == null)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(googleCredentialsPath),
        });
    }
}

builder.Services.AddControllers();

builder.Services.AddSingleton(new FirestoreService(builder.Configuration["FirestoreProjectId"]));

builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", builder =>
    {
        builder.WithOrigins("http://localhost:8080", "https://loan-application-54ea.onrender.com")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();