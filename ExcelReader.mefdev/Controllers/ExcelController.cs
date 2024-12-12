using ExcelReader.mefdev.Models;

namespace ExcelReader.mefdev.Controllers
{
	public class ExcelController: ExcelBase
	{
		public ExcelController()
		{
		}

        public void DisplayFinancialData(List<FinancialData> data)
        {
            if (data == null)
            {
                DisplayMessage("Financial data are not found or Empty", "red");
                return;
            }
            DisplayMessage("Getting data...", "green");
            DisplayAllItems(data);
            return;
        }
    }
}

