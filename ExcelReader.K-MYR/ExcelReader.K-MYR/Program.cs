using ExcelReader.K_MYR;

SQLServerRepo repo = new();
DataReader reader = new(repo);
UserInterface ui = new(reader);

ui.ShowMainMenu();
