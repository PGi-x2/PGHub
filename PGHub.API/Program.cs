using Microsoft.EntityFrameworkCore;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString: "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PGHub;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage(); // This line is commented out to avoid showing the developer exception page
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
