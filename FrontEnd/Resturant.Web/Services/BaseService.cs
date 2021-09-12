using Newtonsoft.Json;
using Resturant.Web.Models;
using Resturant.Web.Services.IServices;
using Resturant.Web.Services.Models;

namespace Resturant.Web.Services
{
    public class BaseService : IBaseService
    {
        // public ServiceResponse<object> response { get ; set; }
        private IHttpClientFactory _httpClient { get; set; }
        public BaseService(IHttpClientFactory httpCliet)
        {
            _httpClient  =httpCliet;
            // response  new ServiceResponse<object>();

        }
        public async Task<ServiceResponse<T>> SendAsync<T>(ApiRequest apiRequest)
        {
            var res = new ServiceResponse<T>();
             try
             {
                 var client = _httpClient.CreateClient("Resturant");
                 var request  = new HttpRequestMessage();
                 request.Headers.Add("Accept", "application/json");
                 request.RequestUri=  new Uri(apiRequest.Url);
                 client.DefaultRequestHeaders.Clear();  
                 if(apiRequest.Data !=null)
                 {
                     request.Content = new StringContent(
                    JsonConvert.SerializeObject(apiRequest.Data)
                    , System.Text.Encoding.UTF8, "application/json");
                 }
                 if(!string.IsNullOrEmpty(apiRequest.Token))
                 {
                     client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
                     
                 }
                 HttpResponseMessage apiResponse = null;
                 switch(apiRequest.ApiType)
                 {
                     case SD.ApiType.POST:
                        request.Method=  HttpMethod.Post;
                        break;
                    case SD.ApiType.GET:
                        request.Method = HttpMethod.Get;
                        break;
                    case SD.ApiType.PUT:
                        request.Method= HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        request.Method= HttpMethod.Delete;
                        break;
                    default : 
                        request.Method = HttpMethod.Get;
                        break;
                 }
                 apiResponse = await client.SendAsync(request);

                 var apiContent = await apiResponse.Content.ReadAsStringAsync();
                 res = JsonConvert.DeserializeObject<ServiceResponse<T>>(apiContent);
                 return res;
             }
             catch (System.Exception ex)
             {
                 res.ErrorMessages = new List<string> {Convert.ToString(ex.Message)};
                 res.Success =  false;
                 var serialize =  JsonConvert.SerializeObject(res);
                 var  response = JsonConvert.DeserializeObject<ServiceResponse<T>>(serialize);
                 return response;
             }
             
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}