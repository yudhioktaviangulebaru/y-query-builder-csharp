using BelajarSharp.configapp;

using Newtonsoft.Json.Serialization;
using QueryBuilderSharp.example.application_parameters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// start dependency injection
builder.Services.AddScoped<AppParamRepo, ApplicationParameterRepository>();
//end dependedncy injection

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{ options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; })
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
var app = builder.Build();
AutoMigration.runMigration();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
