using DataLayer;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using GymApi.Extensions;
using GymApi.Interfaces;
using GymApi.Options;
using GymApi.Services;
using GymApi.Utilites;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtTokenOptions>(
    builder.Configuration.GetSection("JwtTokenOptions"));

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGymClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GymDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
builder.Services.AddScoped<ISetRepository, SetRepository>();
builder.Services.AddScoped<ITrainerClientRepository, TrainerClientRepository>();

builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<TrainerService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WorkoutPlanService>();
builder.Services.AddScoped<WorkoutService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<TokenProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("AllowGymClient");
app.UseJwtAuthentication();
app.MapControllers();

app.Run();
