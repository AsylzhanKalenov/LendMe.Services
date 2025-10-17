using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Lendme.Application;
using Lendme.Infrastructure;
using Lendme.Infrastructure.Hubs;
using Lendme.Web.Feature.AutoMapper;
using Lendme.Web.Feature.Seeder;
using Lendme.Web.Feature.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureApplication();

// SignalR
builder.Services.AddSignalR();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set a default version
    options.AssumeDefaultVersionWhenUnspecified = true; // Use default if no version is provided
    options.ReportApiVersions = true; // Report supported and deprecated API versions in headers

    // Configure the API version reader (choose one or combine multiple)
    // options.ApiVersionReader = new UrlSegmentApiVersionReader(); // From URL path (e.g., /v1/)
    // options.ApiVersionReader = new QueryStringApiVersionReader("api-version"); // From query string (e.g., ?api-version=1.0)
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-API-Version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Format for grouping API versions in Swagger UI
    options.SubstituteApiVersionInUrl = true; // Replace {apiVersion} in URL templates
});

// Add Swagger/OpenAPI generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddScoped<DatabaseSeeder>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await SeedDatabaseAsync(app);
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.MapHub<ChatHub>("/chatHub");
app.MapHub<NotificationHub>("/notificationHub");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

static async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}
