using Microsoft.EntityFrameworkCore; // Importing Entity Framework Core for database operations
using Microsoft.AspNetCore.Mvc; // Importing ASP.NET Core MVC for building APIs
using System.Linq; // Importing LINQ for querying collections


// Represents a Note entity in the application


public class Note
{
    public int Id { get; set; } // Unique identifier for the note
    public required string Content { get; set; } // The content of the note (required field)
}

// Represents the database context for the application
public class NotesContext : DbContext
{
    public DbSet<Note> Notes { get; set; } // Represents the Notes table in the database

    // Configures the database to use SQLite with the specified file
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=notes.db");
}

// Defines a controller for handling API requests related to notes
[ApiController] // Marks this class as an API controller
[Route("api/notes")] // Specifies the base route for this controller
public class NotesController : ControllerBase
{
    private readonly NotesContext _context; // Database context for interacting with the database

    // Constructor that initializes the database context and ensures the database is created
    public NotesController(NotesContext context)
    {
        _context = context;
        _context.Database.EnsureCreated(); // Ensures the database and tables are created
    }

    // Handles GET requests to retrieve all notes
    [HttpGet]
    public IActionResult GetNotes() => Ok(_context.Notes.ToList()); // Returns all notes as a list

    // Handles POST requests to add a new note
    [HttpPost]
    public IActionResult AddNote([FromBody] Note note) // Accepts a Note object from the request body
    {
        _context.Notes.Add(note); // Adds the new note to the database
        _context.SaveChanges(); // Saves changes to the database
        return Created("", note); // Returns a 201 Created response with the created note
    }

    // Handles DELETE requests to delete a note by its ID
    [HttpDelete("{id}")] // Specifies that the ID is passed as a route parameter
    public IActionResult DeleteNote(int id)
    {
        var note = _context.Notes.Find(id); // Finds the note by its ID
        if (note == null) return NotFound(); // Returns 404 Not Found if the note doesn't exist
        _context.Notes.Remove(note); // Removes the note from the database
        _context.SaveChanges(); // Saves changes to the database
        return NoContent(); // Returns a 204 No Content response
    }
}