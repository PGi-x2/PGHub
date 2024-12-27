using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PGHub.Application.Services;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;
using System.Reflection;
using PGHub.Application.Middleware;
using PGHub.Application.DTOs.User.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add controllers to the container.
builder.Services.AddControllers();
builder.Services.AddLogging(); 
builder.Services.AddFluentValidationAutoValidation(); // Add FluentValidation for automatic validation
builder.Services.AddFluentValidationClientsideAdapters(); // Add FluentValidation for client-side validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDTOValidator>(); // Add FluentValidation registers all validators in the assembly containing CreateUserDTOValidator

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PG Hub API", Version = "v1" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add the database context
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString: "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PGHub;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

// Add repositories
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();

// Add application services
builder.Services.AddScoped<IUsersService, UsersService>();

// Add custom services
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Add AutoMapper for mapping between DTOs and entities


// Example: Add MediatR for handling CQRS patterns
// builder.Services.AddMediatR(typeof(Program).Assembly);

// Example: Add FluentValidation for model validation
// builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);  -> Deprecated

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage(); // This line is commented out to avoid showing the developer exception page
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
