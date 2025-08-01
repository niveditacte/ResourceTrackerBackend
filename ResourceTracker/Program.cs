using ResourceTracker.DAO;
using ResourceTracker.DAO.Interfaces;
using ResourceTracker.Orchestration;
using ResourceTracker.Orchestration.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

builder.Services.AddScoped<IResourceTrackerDao, ResourceTrackerDAO>();
builder.Services.AddScoped<IEmployeeDao, EmployeeDao>();
builder.Services.AddScoped<IResourceTrackerOrchestration, ResourceTrackerOrchestation>();
builder.Services.AddScoped<IDropDownDAO, DropDwnDAO>();
builder.Services.AddScoped<IDropDownOrchestration, DropDownOrchestration>();
builder.Services.AddScoped<IEmployeeOrchestration, EmployeeOrchestration>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseDeveloperExceptionPage(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
