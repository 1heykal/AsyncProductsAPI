using AsyncProductAPI.Data;
using AsyncProductAPI.DTOs;
using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=RequestDb.db"));



var app = builder.Build();

app.UseHttpsRedirection();


// Start EndPoint
 app.MapPost("api/v1/products", async (AppDbContext context, ListingRequest listingRequest) =>{
    if(listingRequest is null)
    return Results.BadRequest();

    listingRequest.RequestStatus = "ACCEPT";
    listingRequest.EstimatedCompeletionTime = "2024-02-29:14:00:00";

    await context.ListingRequests.AddAsync(listingRequest);
    await context.SaveChangesAsync();

    return Results.Accepted($"api/v1/productstatus/{listingRequest.RequestId}", listingRequest);
 });

 // Status endpoit
 app.MapGet("api/v1/productstatus/{requestId}", (AppDbContext context, string requestId) => {
   var listingRequest = context.ListingRequests.FirstOrDefault(lr => lr.RequestId == requestId);
   if(listingRequest is null)
   return Results.NotFound();

   ListingStatus listingStatus = new ListingStatus{
    RequestStatus = listingRequest.RequestStatus,
    ResourceURL = string.Empty
   };

   if(listingRequest.RequestStatus!.Equals("COMPLETE")){
    listingStatus.ResourceURL = $"api/v1/products/{Guid.NewGuid()}";
      return Results.Redirect("http://localhost:5035/" + listingStatus.ResourceURL);
   }

   listingStatus.EstimatedCompeletionTime  = "2024-02-29:15:00:00";
   return Results.Ok(listingStatus);

 });

 // Final Endpoint
 app.MapGet("api/v1/products/{requestId}", (string requestId) => {
    return Results.Ok("This is where you pass back the final results.");
 });

app.Run();

