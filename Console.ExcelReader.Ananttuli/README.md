# ExcelReader

- Prompts user for file path (default provided).
- Reads data from any excel sheet, regardless of the number of columns or the content of the header.
- Stores in the database
- Displays data in the console

## Run locally

- `cd <Repo_root>/ExcelReader`
- `dotnet run`

## Configuration (optional)

Default configuration should work as-is, can be customised if
desired:

- Change SQLite database path in `App/App.config` (default should
  work)

## Tech stack

- C#
- EntityFramework Core ORM
- SQLite
