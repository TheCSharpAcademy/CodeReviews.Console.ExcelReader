USE ExcelReaderProgram;


SELECT table_catalog [database], table_schema [schema], table_name name, table_type type
FROM INFORMATION_SCHEMA.TABLES
GO

SELECT * FRom ExcelWorkSheet
GO

SELECT * FRom ExcelRow
GO

SELECT * FROM DateCells
GO