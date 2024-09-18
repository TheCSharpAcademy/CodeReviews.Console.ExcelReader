# ExcelReader

ExcelReader is a C# application that reads data from an Excel file, stores it in a SQLite database, and displays the data in a console table format. This project demonstrates the use of Entity Framework Core, EPPlus for Excel file handling, and Spectre.Console for enhanced console output.

## Features

- Read data from Excel files
- Store data in a SQLite database
- Display data in a formatted console table
- Dependency Injection for better code organization and testability

## Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or any compatible IDE

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/ExcelReader.git
cd ExcelReader
```

### Build the project
```dotnet build```

### Run the Application

```dotnet run --project ExcelReader```

## Project Structure

* Program.cs: Entry point of the application, configures services and initializes the database.
* Controllers/DatabaseController.cs: Manages database operations and coordinates between services.
* Controllers/ExcelReaderController.cs: Handles Excel file reading operations.
* Services/DatabaseService.cs: Provides database-related services.
* Services/ExcelService.cs: Handles Excel file reading using EPPlus.
* Data/AppDbContext.cs: Defines the Entity Framework Core database context.
* Models/DataModel.cs: Represents the data structure for Excel rows and database entries.
* Utilities/Logger.cs: Provides logging and data display functionality.

## Configuration

The application uses a SQLite database. The connection string is defined in AppDbContext.cs:
```optionsBuilder.UseSqlite("Data Source=ExcelToDb.db");```

The Excel file path is currently hardcoded in ExcelService.cs. Update this path to match your Excel file location:
```var filePath = "/path/to/your/excel/file.xlsx";```

## Dependencies

* Microsoft.EntityFrameworkCore.Sqlite
* Microsoft.Extensions.Hosting
* EPPlus
* Spectre.Console

## Contributing

1. Fork the repository
2. Create your feature branch (git checkout -b feature/AmazingFeature)
3. Commit your changes (git commit -m 'Add some AmazingFeature')
4. Push to the branch (git push origin feature/AmazingFeature)
5. Open a Pull Request