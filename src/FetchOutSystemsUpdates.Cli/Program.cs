using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace FetchOutSystemsUpdates.Cli;

public class Program
{
    public static Task<int> Main(string[] args) => TitleCommand.RunAsync(args);
}

public static partial class TitleCommand
{
    private const string DefaultUrl = "https://example.com";

    public static async Task<int> RunAsync(
        string[] args,
        Func<string, Task<string>>? fetchTitleAsync = null,
        TextWriter? output = null,
        TextWriter? error = null)
    {
        output ??= Console.Out;
        error ??= Console.Error;
        fetchTitleAsync ??= FetchTitleWithPlaywrightAsync;

        var url = args.Length > 0 ? args[0] : DefaultUrl;

        try
        {
            var title = await fetchTitleAsync(url);
            await output.WriteLineAsync(title);
            return 0;
        }
        catch (Exception ex)
        {
            await error.WriteLineAsync($"Failed to fetch page title from '{url}': {ex.Message}");
            return 1;
        }
    }

    public static async Task<string> FetchTitleWithPlaywrightAsync(string url)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var request = await playwright.APIRequest.NewContextAsync();
        var response = await request.GetAsync(url);

        if (!response.Ok)
        {
            throw new InvalidOperationException($"Request failed with status code {response.Status}");
        }

        var html = await response.TextAsync();
        var match = TitleRegex().Match(html);
        if (!match.Success)
        {
            throw new InvalidOperationException("No <title> tag found in the response.");
        }

        return match.Groups[1].Value.Trim();
    }

    [GeneratedRegex("<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex TitleRegex();
}
