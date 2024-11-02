namespace ExcelReader.Arashi256.Classes
{ 
    public class ServiceResponseUtils
    {
        public static ServiceResponse CreateResponse(ResponseStatus status, string message, object? data)
        {
            return new ServiceResponse { Status = status, Message = message, Data = data };
        }
    }
}