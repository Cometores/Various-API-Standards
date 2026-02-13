using Minimal.API;
using Minimal.API.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProgramDependencies(builder.Configuration);

var app = builder.Build();
app.ConfigurePipeline();

Seeder.RecreateDatabase(app);

app.Run();