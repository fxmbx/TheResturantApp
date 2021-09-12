namespace Resturant.Web.Services.Models
{
    public class ServiceResponse <T>
    {
        public T Data {get; set;}

        public bool Success {get; set;} = true;

        public string? Message {get; set;} = "Something Went Wrong";

        public List<string>? ErrorMessages {get; set;}
    }
}