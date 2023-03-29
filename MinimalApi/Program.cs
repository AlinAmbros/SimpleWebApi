using Microsoft.EntityFrameworkCore;
using MinimalApi;

var builder = WebApplication.CreateBuilder();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/contacts", async (ApplicationContext db) => await db.Contacts.ToListAsync());

app.MapGet("/api/contacts/{id:int}", async (int id, ApplicationContext db) =>
{
    Contact? contact = await db.Contacts.FirstOrDefaultAsync(t => t.Id == id);
    if (contact == null) return Results.NotFound(new { message = "The contact is not found" });

    return Results.Json(contact);
});

app.MapDelete("/api/contacts/{id:int}", async (int id, ApplicationContext db) =>
{
    Contact? contact = await db.Contacts.FirstOrDefaultAsync(t => t.Id == id);
    if (contact == null) return Results.NotFound(new { message = "The contact is not found" });

    db.Contacts.Remove(contact);
    await db.SaveChangesAsync();
    return Results.Json(contact);
});

app.MapPost("/api/contacts", async (Contact contact, ApplicationContext db) =>
{
    await db.Contacts.AddAsync(contact);
    await db.SaveChangesAsync();
    return contact;
});

app.MapPut("/api/contacts/{id:int}", async (int id, Contact contactData, ApplicationContext db) =>
{
    var contact = await db.Contacts.FirstOrDefaultAsync(u => u.Id == id);
    if (contact == null) return Results.NotFound(new { message = "The contact is not found" });

    contact.Name = contactData.Name;
    contact.Surname = contactData.Surname;
    contact.Email = contactData.Email;
    contact.DateOfBirth = contactData.DateOfBirth;
    contact.PhoneNumber = contactData.PhoneNumber;
    contact.Country = contactData.Country;
    contact.City = contactData.City;

    await db.SaveChangesAsync();
    return Results.Json(contact);
});

app.Run();

