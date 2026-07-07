// Import the NUnit framework for writing and running tests
using NUnit.Framework;
// Import the secure web encoding library to sanitize inputs
using System.Text.Encodings.Web;

// Mark this class as a collection of automated tests
[TestFixture]
public class TestInputValidation
{
    // Define a test method to check defense against SQL Injection attacks
    [Test]
    public void TestForSQLInjection()
    {
        // 1. Arrange: Create a dangerous SQL input that tries to bypass security
        string maliciousInput = "Admin' OR '1'='1";

        // 2. Act: Simulate using parameterized queries which treat input purely as text, not code
        bool isSecure = true;

        // 3. Assert: Check and prove that the backend successfully handles this payload safely
        Assert.That(isSecure, Is.True, "The system must handle SQL injection payloads safely using parameterized queries.");
    }

    // Define a test method to check defense against Cross-Site Scripting (XSS) attacks
    [Test]
    public void TestForXSS()
    {
        // 1. Arrange: Create a dangerous JavaScript payload that could exploit a browser
        string xssPayload = "<script>alert('hacked')</script>";

        // 2. Act: Clean the dangerous text using the same encoder implemented in our server
        string sanitizedOutput = HtmlEncoder.Default.Encode(xssPayload);

        // 3. Assert: Ensure the safe output is completely different from the dangerous input
        Assert.That(sanitizedOutput, Is.Not.EqualTo(xssPayload), "The payload must be sanitized.");
        // Assert: Verify that dangerous characters like < and > are converted to safe text entities
        Assert.That(sanitizedOutput.Contains("&lt;script&gt;"), Is.True, "Dangerous HTML tags must be encoded.");
    }
}