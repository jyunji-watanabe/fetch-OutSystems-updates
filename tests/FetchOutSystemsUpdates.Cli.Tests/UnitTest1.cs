using System.IO;
using System.Threading.Tasks;
using FetchOutSystemsUpdates.Cli;

namespace FetchOutSystemsUpdates.Cli.Tests;

public class UnitTest1
{
    [Fact]
    public async Task RunAsync_UsesDefaultUrl_WhenNoArgumentsAreProvided()
    {
        var output = new StringWriter();
        var capturedUrl = string.Empty;

        var exitCode = await TitleCommand.RunAsync(
            [],
            url =>
            {
                capturedUrl = url;
                return Task.FromResult("Example Domain");
            },
            output,
            new StringWriter());

        Assert.Equal(0, exitCode);
        Assert.Equal("https://example.com", capturedUrl);
        Assert.Equal("Example Domain", output.ToString().Trim());
    }

    [Fact]
    public async Task RunAsync_ReturnsError_WhenFetchingFails()
    {
        var error = new StringWriter();

        var exitCode = await TitleCommand.RunAsync(
            ["https://example.com"],
            _ => Task.FromException<string>(new InvalidOperationException("boom")),
            new StringWriter(),
            error);

        Assert.Equal(1, exitCode);
        Assert.Contains("Failed to fetch page title", error.ToString());
    }
}
