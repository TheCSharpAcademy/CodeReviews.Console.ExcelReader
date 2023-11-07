namespace ExcelReader.Forser.Services
{
    internal class ExcelServices : IExcelService
    {
        private readonly IExcelRepository _excelRepository;
        public ExcelServices(IExcelRepository excelRepository)
        {
            _excelRepository = excelRepository;
        }
        public void AddPlayers(IEnumerable<HockeyModel> hockeyPlayers)
        {
            _excelRepository.AddPlayers(hockeyPlayers);
        }
        public List<HockeyModel> DisplayAllPlayers()
        {
            return _excelRepository.GetAllPlayers().ToList();
        }
    }
}