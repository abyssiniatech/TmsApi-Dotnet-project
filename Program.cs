


// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();

// var app = builder.Build();

// app.UseMiddleware<RequestLoggingMiddleware>();

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllers();

// app.Run();










// var builder = WebApplication.CreateBuilder(args);


// builder.Services.AddControllers();


// // builder.Services
//     .AddAuthentication("Training")
//     .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>(
//         "Training",
//         options => { });

// // Authorization
// builder.Services.AddAuthorization();

// var app = builder.Build();

// // Middleware pipeline

// app.UseMiddleware<RequestLoggingMiddleware>(); // make sure this exists

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllers();

// app.Run();





var builder = WebApplication.CreateBuilder(args);

// Add controllers (required for WeatherForecastController)
builder.Services.AddControllers();

var app = builder.Build();

// Development helper (optional but useful)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Routing controllers
app.MapControllers();

app.Run();