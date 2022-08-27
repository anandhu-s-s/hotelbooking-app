using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(option=>option.AddDefaultPolicy(
    option=>option.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
));
builder.Services.AddDbContext<RoomDb>(opt => opt.UseInMemoryDatabase("RoomList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<NewBookingDb>(opt => opt.UseInMemoryDatabase("NewBookingList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();
app.UseCors();
//.....................NewBooking.......................//

app.MapGet("/NewBooking", async (NewBookingDb db) =>
    await db.NewBookings.ToListAsync());



app.MapPost("/NewBooking", async (NewBooking newbooking,NewBookingDb db)=>
{
    db.NewBookings.Add(newbooking);
    await db.SaveChangesAsync();

    return Results.Created($"/NewBooking/{newbooking.Id}", newbooking);
}
);
app.MapPut("/NewBooking/{id}", async (int id, NewBooking inputNewbooking, NewBookingDb db) =>
{
    var newBooking = await db.NewBookings.FindAsync(id);

    if (newBooking is null) return Results.NotFound();
     
    newBooking.FirstName = inputNewbooking.FirstName;
    newBooking.LastName = inputNewbooking.LastName;
    newBooking.CheckIn = inputNewbooking.CheckIn;
    newBooking.CheckOut = inputNewbooking.CheckOut;
    newBooking.noOfchildren=inputNewbooking.noOfchildren;
     newBooking.noOfadults=inputNewbooking.noOfadults;
    
  
    await db.SaveChangesAsync();

    return Results.NoContent();
});
app.MapDelete("/NewBooking/{id}", async (int id, NewBookingDb db) =>
{
    if (await db.NewBookings.FindAsync(id) is NewBooking newbooking)
    {
        db.NewBookings.Remove(newbooking);
        await db.SaveChangesAsync();
        return Results.Ok(newbooking);
    }

    return Results.NotFound();
});
   
//.........................Rooms.........................//
app.MapGet("/room", async (RoomDb db) =>
    await db.rooms.ToListAsync());

app.MapPost("/room", async (Room room, RoomDb db) =>
{
    db.rooms.Add(room);
    await db.SaveChangesAsync();

    return Results.Created($"/room/{room.Id}", room);
});

app.MapPut("/room/{id}", async (int id, Room inputRoom, RoomDb db) =>
{
    var room = await db.rooms.FindAsync(id);

    if (room is null) return Results.NotFound();
     
    room.Roomno = inputRoom.Roomno;
    room.Adultno = inputRoom.Adultno;
     room.childno = inputRoom.childno;
      room.price = inputRoom.price;


    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/room/{id}", async (int id, RoomDb db) =>
{
    if (await db.rooms.FindAsync(id) is Room room)
    {
        db.rooms.Remove(room);
        await db.SaveChangesAsync();
        return Results.Ok(room);
    }

    return Results.NotFound();
});



app.Run();
//....................................Room....................................//
class Room
{
    public int Id { get; set; }
    public int Roomno { get; set; }
    public int Adultno { get; set; }
    public int childno { get; set; }
    public int price { get; set; }
   
}

class RoomDb : DbContext
{
    public RoomDb(DbContextOptions<RoomDb> options)
        : base(options) { }

    public DbSet<Room> rooms => Set<Room>();
}

//...................NewBooking............................//
class NewBookingDb : DbContext
{
    public NewBookingDb(DbContextOptions<NewBookingDb> options)
        : base(options) { }

    public DbSet<NewBooking> NewBookings => Set<NewBooking>();
}


class NewBooking{

    public int Id { get; set; }
     public string? FirstName { get; set; }
      public string? LastName { get; set; }
      public string? CheckIn { get; set; }
       public string? CheckOut { get; set; }
       public int noOfadults { get; set; }
       public int noOfchildren { get; set; }
      


}
