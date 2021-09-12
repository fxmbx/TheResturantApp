
using static Resturant.Web.SD;

namespace Resturant.Web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; }

        public object? Data { get; set; }

        public string? Token { get; set; }
    }
}