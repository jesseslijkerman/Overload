using Microsoft.EntityFrameworkCore;
using Overload.Api.Data;
using Overload.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<OverloadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IExerciseService, ExerciseService>();
// builder.Services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();
// builder.Services.AddScoped<ISetEntryService, SetEntryService>();

var app = builder.Build();

// Update database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OverloadDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();