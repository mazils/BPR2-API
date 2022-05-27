
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudyIt.MongoDB.Models;
using StudyIt.MongoDB.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS Accepting every request.
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

// Add services to the container for DI.
builder.Services.Configure<StudyItDatabaseSettings>(
builder.Configuration.GetSection("StudyItDatabase"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IProjectGroupService, ProjectGroupService>();
builder.Services.AddScoped<ISearchService, SearchService>();


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
