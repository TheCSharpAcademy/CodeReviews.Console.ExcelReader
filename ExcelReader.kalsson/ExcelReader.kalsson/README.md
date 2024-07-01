# Excel Reader

Excel Reader is a C# console application that reads data from an Excel spreadsheet and stores it in a SQLite database 
using Entity Framework. The application then retrieves and displays the data from the database in the console using 
Spectre.Console for a more visually appealing presentation.

## Features

- Reads data from an Excel file (`esd.xlsx`)
- Stores the data in a SQLite database
- Displays the data in the console using a formatted table

## Requirements

- .NET 8.0 SDK
- EPPlus
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design
- Spectre.Console

## Project Structure

```bash
ExcelReader.kalsson/
├── Data/
│ └── AppDbContext.cs
├── Models/
│ └── EmployeeModel.cs
├── Services/
│ └── ExcelService.cs
├── esd.xlsx
├── Program.cs
├── ExcelReader.kalsson.csproj
└── README.md
```

## Getting Started

### Prerequisites

- Ensure you have .NET 8.0 SDK installed. You can download it from the [.NET website](https://dotnet.microsoft.com/download/dotnet/8.0).
- Ensure you have a compatible IDE installed, such as JetBrains Rider or Visual Studio.

### Setup

1. **Clone the repository**.
2. **Restore dependencies**:

```bash
dotnet restore
```

3. **Build the project**:

```bash
dotnet build
```

### Running the Application

1. Ensure esd.xlsx is in the project directory where Program.cs is located.
2. Run the application from the command line:

```bash
dotnet run
```

Alternatively, you can run the application from within your IDE.

## How It Works

1. Reading Data from Excel:
* The application uses the EPPlus library to read data from esd.xlsx.

2. Storing Data in SQLite:

* The data is stored in a SQLite database using Entity Framework Core.

3. Displaying Data:

* The application retrieves the data from the SQLite database and displays it in a formatted table using 
Spectre.Console.