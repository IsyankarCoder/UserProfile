using Microsoft.EntityFrameworkCore;
using UserProfile.DataBase;
using UserProfile.Interfaces;
using UserProfile.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


builder.Services.AddDbContext<UserProfileContext>(
  options=>{
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
      options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

  } 
);

builder.Services.AddScoped<IUserService,UserService>();

app.MapGet("/", () => "Hello World!");

app.Run();
