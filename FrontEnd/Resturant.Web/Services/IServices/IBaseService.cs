using Resturant.Web.Models;
using Resturant.Web.Services.Models;

namespace Resturant.Web.Services.IServices
{
    public interface IBaseService : IDisposable 
    {
        // ServiceResponse<object> response {get; set;}
         Task<ServiceResponse<T>> SendAsync<T>(ApiRequest ApiRequest);
         
    }
}