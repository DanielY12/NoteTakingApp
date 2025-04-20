// Create a WebApplication builder to configure the application
var builder = WebApplication.CreateBuilder(args);

// Add support for controllers to the service container
builder.Services.AddControllers();

// Register the NotesContext as a service for dependency injection
builder.Services.AddDbContext<NotesContext>();

// Configure CORS (Cross-Origin Resource Sharing) to allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Allow requests from any origin
              .AllowAnyMethod() // Allow any HTTP method (GET, POST, DELETE, etc.)
              .AllowAnyHeader(); // Allow any headers in the request
    });
});

// Build the application
var app = builder.Build();

// Enable the configured CORS policy named "AllowAll"
app.UseCors("AllowAll");

// Map controller routes to handle incoming HTTP requests
app.MapControllers();

// Run the application
app.Run();