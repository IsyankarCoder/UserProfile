using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserProfile.DataBase;
using UserProfile.Interfaces;
using UserProfile.Models;
using UserProfile.Responses;
using UserProfile.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<UserProfileContext>(
  options=>{
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
      options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

  },ServiceLifetime.Scoped 
);

builder.Services.AddTransient<IUserService,UserService>();
var app = builder.Build();


var users = app.MapGroup("/users");
var response = new CommonResponse();

users.MapGet("/",async (IUserService _userService)=>{
      
      try
      {
        var data = await _userService.GetAllUsers();
        response.Code=200;
        response.Message="Success Get User List";
        response.Data=data;
        return Results.Ok(response);
      }
      catch (System.Exception ex)
      {
        response.Code=400;
        response.Message =$"Failed To Get User";
        response.Data=ex.Message;
        return Results.BadRequest(response);
      }
      
});


users.MapPost("/",async (IUserService _userService,[FromBody] UserModel model)=>{
     try
     {
       var data = await _userService.CreateUser(model);
       response.Code=200;
       response.Data=data;
       response.Message=$"Success Added User {model.UserName}";
       return Results.Ok(response);
     }
     catch (System.Exception ex)
     {
        response.Code=400;
        response.Message=$"Failed to Add User";
        response.Data=ex.Message;
        return Results.BadRequest(response);
     }
});


users.MapGet("/{id}", async (IUserService _userService,int Id)=>{
    try
    {
       var data = await CheckUser(Id,_userService);
       if(data is null){
         response.Code = StatusCodes.Status404NotFound;
         response.Message = "User Not Found";
         response.Data= MessageNotFound(Id);
         return Results.Ok(response);
       }

       response.Code=200;
       response.Message="Success Get User Detail";
       response.Data =data;
       return Results.Ok(response);
    }
    catch (System.Exception ex)
    {
      response.Code=400;
      response.Message = $"Failed to get user";
      response.Data = ex.Message;
      return Results.BadRequest(response); 
    }
});

users.MapDelete("/{id}",async (IUserService _userService,int Id)=>{
     try
     {
        var checkUser = await CheckUser(Id,_userService);
        if(checkUser is null){
          response.Code= StatusCodes.Status404NotFound;
          response.Message = "User Not Found";
          response.Data = MessageNotFound(Id);
          return Results.NotFound(response);
        }

        var data = await _userService.DeleteUser(Id);
        response.Code=200;
        response.Message=$"Success Deleted User";
        response.Data = data;
        return Results.Ok(response);
     }
     catch (System.Exception ex)
     {
        response.Code=400;
        response.Message=$"Failed to delete user";
        response.Data= ex.Message;
        return Results.BadRequest(response); 
     }
});

users.MapPut("/", async (IUserService _userService,[FromBody] UserModel userModel)=>{
  try
  {
     var checkUser  = await CheckUser(userModel.Id,_userService);
     if(checkUser is null){
       response.Code = StatusCodes.Status404NotFound;
       response.Message="User Not Found";
       response.Data=MessageNotFound(userModel.Id);
       return Results.NotFound(response);
     }

     var data  = await  _userService.UpdateUser(userModel);
     response.Code=200;
     response.Message= $"Success Updated User {userModel.UserName}";
     response.Data = data;
     return Results.Ok(response);

     
  }
  catch (System.Exception ex)
  {
     response.Code = 400;
        response.Message = $"Failed to update user";
        response.Data = ex.Message;
        return Results.BadRequest(response);
  }
});


static string MessageNotFound(int Id){
  return $"User with id {Id} not found";
}

static async Task<UserModel?> CheckUser(int Id, IUserService _userService){
  var data = await _userService.GetUserDetail(Id);
  return data;
}

app.Run();
