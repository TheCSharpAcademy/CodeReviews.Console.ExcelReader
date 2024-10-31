namespace ExcelReader.Arashi256.Classes
{
    public class ServiceResponse
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.Failure;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public enum ResponseStatus { Success, Failure }
}