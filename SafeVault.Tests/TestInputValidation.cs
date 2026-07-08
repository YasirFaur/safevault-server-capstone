namespace SafeVault;
// Import the NUnit framework to run tests
using NUnit.Framework;
// Import our server project to use the InputSanitizer class
using SafeVault;

// Mark this class as a collection of automated tests
[TestFixture]
public class TestInputValidation
{
    // Define a test for checking SQL Injection defense
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

    // Define a test for checking Cross-Site Scripting (XSS) defense
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
}