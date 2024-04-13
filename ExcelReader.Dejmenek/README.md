﻿
# ExcelReader

## Table of Contents
- [General Info](#general-info)
- [Technologies](#technologies)
- [Features](#features)

## General Info
This project is a C# application that reads data from an Excel file containing inventory item information and inserts it into a SQL Server database.
## Technologies
- C#
- EPPlus
- Dapper
- SQL Server
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)

## Features
- Reads Excel Files: The application can read data from Excel files in the .xlsx format
- Maps Data to Inventory Items: It parses the data and creates corresponding inventory item objects based on the columns in the Excel sheet.
- Inserts Data into Database: