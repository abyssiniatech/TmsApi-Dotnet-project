using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CAPTIVE DEPENDENCY VALIDATION (Session 2 Requirement) ---
// Catches scope mismatches at startup, checking items 4 & 5 on your verification sheet
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

// --- 2. SERVICE LIFETIME REGISTRATIONS (Session 2 & 3 Foundations) ---
// Registrations mapped precisely to their required thread-safe/request boundaries
builder.Services.AddControllers(); 

// Configuration & Options Pattern validation (Fulfills verification item 6)
builder.Services.Configure<PaymentOptions>(builder.Configuration.GetSection("Payments"));
builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateOnStart(); 

// Core Data and Business Layer services (Must be Scoped, never Singleton)
builder.Services.AddDbContext<TMSDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// Custom Security Infrastructure Setup
builder.Services.AddAuthentication("TrainingScheme")
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>("TrainingScheme", null);

builder.Services.AddAuthorization();
builder.Services.AddProblemDetails(); 
builder.Services.AddOpenApi(); 


var app = builder.Build();

// --- 3. MIDDLEWARE EXECUTION PIPELINE ---

// Logging always occupies the outermost execution ring
app.UseMiddleware<RequestLoggingMiddleware>();

// Environment-Aware Posture Switching
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages(); 
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

// Route Mapping Engine
app.MapControllers(); 

// Test Error Handler Endpoint
app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException("Simulated database failure for ProblemDetails testing");
});

// Minimal API Protected Test Endpoint
app.MapGet("/api/assessments/results", (ClaimsPrincipal user) => 
{
    return Results.Ok(new
    {
        courseCode = "CS-101",
        studentId = user.Identity?.Name ?? "Unknown",
        letterGrade = "A"
    });
}).RequireAuthorization();

app.Run();
