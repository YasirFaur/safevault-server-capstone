using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient; // Library to connect to SQL Server
// Import the SafeVault namespace to use the InputSanitizer class from another file.
using SafeVault;

// Create a builder to configure and setup our web application.
var builder = WebApplication.CreateBuilder(args);

// Build the application using the configurations from the builder.
var app = builder.Build();

// Enable the server to read and serve static files like HTML from the wwwroot folder.
app.UseStaticFiles();

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SafeVaultDB;Integrated Security=True
// Paste your Connection String here
string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SafeVaultDB;Integrated Security=True;";

// Create a simple route for the main page to show a text message.
app.MapGet("/", () => "SafeVault Server is Running!");
/*
Explanation of this line:
- app.MapGet: Means "Hey server, when a user asks for a page...".
- "/": Means the main page of the website.
- () =>: Means "Run the next command right now".
- "SafeVault is running!": The text message to show to the user.
*/

// Create a route to receive data from the HTML form using POST method.
// Update the POST route to validate and sanitize user inputs.
app.MapPost("/submit", (HttpContext context) => {
    // 1. Read input data from the form.
    string? username = context.Request.Form["username"];
    /*
        Explanation of the '?' symbol:
        - string?: Means the variable can hold a real text OR it can be "null" (empty).
        - Why use it? It protects our application from crashing if the user sends no data.
    */
    string? email = context.Request.Form["email"];

    // 2. Validate: Check if inputs are empty.
    if (string.IsNullOrWhiteSpace(username) || 
    string.IsNullOrWhiteSpace(email))
    {
        context.Response.StatusCode = 400; // Bad Request
        return "Error: Username and Email cannot be empty!";
    }

    // 3. Sanitize: Clean inputs to prevent XSS attacks.
    string safeUsername = InputSanitizer.SanitizeInput(username);
    string safeEmail = InputSanitizer.SanitizeInput(email);

    // SQL Query using Parameterized input (@user, @mail) to prevent SQL Injection
    string query = "INSERT INTO Users (Username, Email) VALUES (@user, @mail)";

    // Open connection and execute securely
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            // Bind values safely to the parameters
            command.Parameters.AddWithValue("@user", safeUsername);
            command.Parameters.AddWithValue("@mail", safeEmail);

            connection.Open();
            command.ExecuteNonQuery(); // Run the SQL command to save data
        }
    }

    // 4. Return the safe and cleaned data.
    return $"Securely Received - Username: {safeUsername}, Email: {safeEmail}";
});

/*
Explanation of this block:
- app.MapPost: Means "Hey server, receive data sent from a form...".
- "/submit": The safe path where the form sends its data.
- HttpContext context: The tool we use to read the request details.
- context.Request.Form["..."]: Finds the exact input field by its name.

- HtmlEncoder.Default.Encode: Changes dangerous characters like '<' or '>' into safe text.
- string.IsNullOrWhiteSpace: Checks if the user left the input blank or typed spaces.
- StatusCode = 400: Tells the browser that the sent data is wrong or incomplete.
*/

// Start the application and keep the server running to listen for requests.
app.Run();