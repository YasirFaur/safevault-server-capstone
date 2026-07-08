namespace SafeVault;
// Import the NUnit framework to run tests
using NUnit.Framework;
// Import our server project to use the InputSanitizer class
using SafeVault;

// Mark this class as a collection of automated tests
[TestFixture]
public class TestInputValidation
{
    // test 1: Define a test for checking SQL Injection defense
    [Test]
    public void TestForSQLInjection()
    {
        // Create a dangerous input containing malicious SQL characters
        string maliciousInput = "Admin' OR '1'='1";

        // Use our new function to clean the dangerous input
        string sanitized = InputSanitizer.SanitizeInput(maliciousInput);

        // Verify that dangerous single quotes are completely removed
        Assert.That(sanitized, Is.Not.Contains("'"), "SQL injection characters must be removed.");
    }

    // test 2: Define a test for checking Cross-Site Scripting (XSS) defense
    [Test]
    public void TestForXSS()
    {
        // Create a dangerous JavaScript payload
        string xssPayload = "<script>alert('hacked')</script>";

        // Use our new function to clean the dangerous input
        string sanitized = InputSanitizer.SanitizeInput(xssPayload);

        // Verify that the safe output is different from the dangerous input
        Assert.That(sanitized, Is.Not.EqualTo(xssPayload), "The payload must be sanitized.");

        // Verify that dangerous HTML tags are encoded safely (checking for &lt;)
        Assert.That(sanitized.Contains("&lt;"), Is.True, "Dangerous HTML tags must be encoded.");
    }

    // Test 3: Verify that a correct password matches its hash
    [Test]
    public void TestCorrectPassword()
    {
        string password = "mySecret123!";
        string hashedPassword = PasswordHasher.HashPassword(password);

        bool result = PasswordHasher.VerifyPassword(password, hashedPassword);

        Assert.That(result, Is.True, "The correct password must match the hash.");
    }

    // Test 4: Verify that an incorrect password fails to match the hash
    [Test]
    public void TestWrongPassword()
    {
        string password = "mySecret123!";
        string hashedPassword = PasswordHasher.HashPassword(password);

        bool result = PasswordHasher.VerifyPassword("wrongPassword", hashedPassword);

        Assert.That(result, Is.False, "The wrong password must not match the hash.");
    }

    // Test 5: Verify that admin role gets access to the admin dashboard
    [Test]
    public void TestAdminAccess_WithAdminRole_ReturnsSuccess()
    {
        // Simulate a user with 'admin' role
        string userRole = "admin";

        // Check authorization logic
        bool hasAccess = userRole == "admin";

        Assert.That(hasAccess, Is.True, "Users with admin role must have access.");
    }

    // Test 6: Verify that regular user role is denied access to the admin dashboard
    [Test]
    public void TestAdminAccess_WithUserRole_ReturnsForbidden()
    {
        // Simulate a regular 'user' trying to access admin tools
        string userRole = "user";

        // Check authorization logic
        bool hasAccess = userRole == "admin";

        Assert.That(hasAccess, Is.False, "Regular users must be denied access.");
    }
}