# ExcelReader.mefdev

## Overview

This application reads data from an Excel file and saves it to a database then display it to the client.

## Features

- Read data from Excel files
- Save data to a database
- Read data from a database

## Requirements

- .NET 8.0 or later
- ExcelDataReader library
- A database (e.g., SQL Server, MySQL)

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Mefdev/ExcelReader.mefdev.git
    ```
2. Navigate to the project directory:
    ```sh
    cd ExcelReader.mefdev
    ```
3. Install the required packages:
    ```sh
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package EppPlus
    ```
    dotnet restore
    ```

## Usage

1. Update the `appsettings.json` file with your database connection string.
2. Run the application:
    ```sh
    dotnet run
    ```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.