using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowFlake.Azure.BlobsStorageService;
using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetValue<string>("AzureBlobStorageConnectionString")));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
//get the configuration 
// var connectionString = builder.Configuration.GetConnectionString("SnowFlakeDbContext");
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddDbContext<SnowFlakeDbContext>(options =>
options.UseMongoDB(mongoDBSettings.AtlasUrl ?? "", mongoDBSettings.DatabaseName ?? ""));

//register the SnowFlakeDbContext with connectionString of appSetting.json
//builder.Services.AddDbContext<SnowFlakeDbContext>(o => o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddTransient<IPlaygroundService, PlaygroundService>();
builder.Services.AddSingleton<ITimerService, TimerService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapHub<TimerHub>("timer-hub");
app.Run();