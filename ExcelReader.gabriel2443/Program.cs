using ExcelReader;
using ExcelReader.Data;

var reader = new Reader();
var createOrDeleteDb = new DatabaseCreateOrDelete();
createOrDeleteDb.CreateOrDeleteDb();
reader.ReadData();
reader.ShowEmployeeDetails();