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

// Serve the main webform HTML file at the root URL
app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/webform.html");
});

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

// Endpoint for secure user login and verification
app.MapPost("/login", (HttpContext context) => {
    // 1. Read input data from the form
    string? username = context.Request.Form["username"];
    string? password = context.Request.Form["password"];

    // 2. Validate: Check if inputs are empty
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
    {
        context.Response.StatusCode = 400; // Bad Request
        return "Error: Username and Password cannot be empty!";
    }

    // 3. Sanitize inputs to prevent injection and XSS
    string safeUsername = InputSanitizer.SanitizeInput(username);
    string safePassword = InputSanitizer.SanitizeInput(password);

    // 4. Simulate a hashed password retrieved from the database for testing
    // In a real app, you would fetch this from SQL using safeUsername
    string mockHashedPasswordFromDb = PasswordHasher.HashPassword("mySecret123!");

    // 5. Verify the entered password against the secure hash
    bool isPasswordValid = PasswordHasher.VerifyPassword(safePassword, mockHashedPasswordFromDb);

    // 6. Return the authentication result
    if (isPasswordValid)
    {
        return $"Login Successful for user: {safeUsername}";
    }
    else
    {
        context.Response.StatusCode = 401; // Unauthorized
        return "Error: Invalid credentials!";
    }
});
// Endpoint for Admin Dashboard - Restricted by Role

app.MapGet("/admin", (HttpContext context) => {
    // 1. Simulate getting the user's role from session or token
    // In a real app, this comes from a secure JWT token or Session
    string userRole = context.Request.Headers["X-User-Role"].ToString();

    // 2. Authorization Check: Only "admin" can enter
    if (userRole != "admin")
    {
        context.Response.StatusCode = 403; // Forbidden
        return "Access Denied: You do not have admin privileges!";
    }

    // 3. Grant access if the role matches
    return "Welcome to the Admin Dashboard! Confidential data loaded.";
});
// Start the application and keep the server running to listen for requests.
app.Run();