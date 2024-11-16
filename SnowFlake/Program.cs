using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Services;
using SnowFlake.UnitOfWork;
using ServerVersion = Microsoft.EntityFrameworkCore.ServerVersion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//get the configuration 
// var connectionString = builder.Configuration.GetConnectionString("SnowFlakeDbContext");

var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddDbContext<SnowFlakeDbContext>(options =>
options.UseMongoDB(mongoDBSettings.AtlasUrl ?? "", mongoDBSettings.DatabaseName ?? ""));

//register the SnowFlakeDbContext with connectionString of appSetting.json
//builder.Services.AddDbContext<SnowFlakeDbContext>(o => o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IPlayerService, PlayerService >();
builder.Services.AddTransient<ITeamService, TeamService >();
// Add services to the container.
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
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//     // Maps controller endpoints
//});
app.Run();