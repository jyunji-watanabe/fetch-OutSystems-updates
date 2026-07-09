# fetch-OutSystems-updates

Fetch OutSystems update information from Product Releases and Updates and Release Notes.

## Boilerplate setup

This repository now includes a .NET solution with:

- Command-line app: `/src/FetchOutSystemsUpdates.Cli`
- xUnit test project: `/tests/FetchOutSystemsUpdates.Cli.Tests`
- Playwright .NET integration in the CLI project

### Run

```bash
dotnet run --project src/FetchOutSystemsUpdates.Cli -- https://example.com
```

### Test

```bash
dotnet test FetchOutSystemsUpdates.slnx
```
