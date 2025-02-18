using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using SnowFlake.Azure.BlobsStorageService;
using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Hubs;
using SnowFlake.Managers;
using SnowFlake.Services;
using SnowFlake.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(120);
        options.KeepAliveInterval = TimeSpan.FromSeconds(40);
    });
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetValue<string>("AzureBlobStorageConnectionString")));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddDbContext<SnowFlakeDbContext>(options =>
options.UseMongoDB(mongoDBSettings.AtlasUrl ?? "", mongoDBSettings.DatabaseName ?? ""));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IPlayerManager, PlayerManager>();
builder.Services.AddTransient<ITeamManager, TeamManager>();
builder.Services.AddTransient<IPlaygroundManager, PlaygroundManger>();
builder.Services.AddTransient<ILeaderboardManager, LeaderboardManager>();
builder.Services.AddTransient<ITransactionManager, TransactionManager>();
builder.Services.AddTransient<ICartManager, CartManager>();
builder.Services.AddTransient<IPlayerManager, PlayerManager>();
builder.Services.AddTransient<IShopManager, ShopManager>();
builder.Services.AddTransient<IImageManager, ImageManager>();

builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddTransient<IGameStateService, GameStateService>();
builder.Services.AddTransient<IPlaygroundService, PlaygroundService>();
builder.Services.AddTransient<ILeaderboardService, LeaderboardService>();
builder.Services.AddSingleton<ITimerService, TimerService>();
builder.Services.AddTransient<IShopService, ShopService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<ICartService, CartService>();


builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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