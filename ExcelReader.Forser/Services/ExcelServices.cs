namespace ExcelReader.Forser.Services
{
    internal class ExcelServices : IExcelService
    {
        private readonly IExcelRepository _excelRepository;
        public ExcelServices(IExcelRepository excelRepository)
        {
            _excelRepository = excelRepository;
        }
        public void AddPlayer(HockeyModel hockeyPlayer)
        {
            throw new NotImplementedException();
        }
        public List<HockeyModel> DisplayAllPlayers()
        {
            return _excelRepository.GetAllPlayers().ToList();
        }
    }
}