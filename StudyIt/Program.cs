
using StudyIt;
using StudyIt.MongoDB;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;


var builder = WebApplication.CreateBuilder(args);
//var firebase = new Firebase();

// CORS Test
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(option =>
{
    option.AddPolicy(MyAllowSpecificOrigins,
        builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.Configure<StudyItDatabaseSettings>(
    builder.Configuration.GetSection("StudyItDatabase"));


//firebase.initializeFirebase();

// TODO Remove
builder.Services.AddSingleton<LoginService>();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CompanyService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
