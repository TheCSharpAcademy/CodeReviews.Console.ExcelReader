using ExcelReader.Forser.UI;

namespace ExcelReader.Forser.Controllers
{
    internal class ExcelController : IExcelController
    {
        private readonly IExcelService _excelService;
        private readonly IUserInterface _excelUI;

        public ExcelController(IExcelService excelService, IUserInterface excelUI)
        {
            _excelService = excelService;
            _excelUI = excelUI;
        }
        public void Run()
        {
            if (_excelService == null)
            {
                throw new ArgumentNullException();
            }

            while(true)
            {
                _excelUI.RenderTitle("Hockey Players");
                //_excelUI.DisplayAllPlayers(_excelService.DisplayAllPlayers());
            }
        }
    }
}