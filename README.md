# SlowBurn (Blazor .NET 8 + Tailwind v4)

## Run
    npm install
    npm run build:css      # or: npm run watch:css (second terminal)
    dotnet run             # or: dotnet watch

## For each new theme, edit in this order
1. `Styles/app.css`      -> @theme tokens (palette, fonts), then component classes
2. `Models/`             -> entities, enums, form classes
3. `Data/SampleData.cs`  -> static data built from the models
4. `Components/Layout/`  -> MainLayout + NavMenu (brand name, links)
5. `Components/Pages/`   -> Home, Items (interactive), Contact (form); add/rename pages

## Rename the project
Find & replace `SlowBurn` -> `YourName` in: SlowBurn.csproj (rename file), Program.cs,
Components/_Imports.razor, Models/*, Data/*. Also set `name` in package.json.
