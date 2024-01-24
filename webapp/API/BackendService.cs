using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace webapp.API
{
    public class BackendService
    {
        private readonly IHttpClientFactory _clientFactory;
        //private readonly AuthenticationService _authService;

        public BackendService(IHttpClientFactory clientFactory/*, AuthenticationService authService*/)
        {
            _clientFactory = clientFactory;
            //_authService = authService;
        }

        public async Task<string> CallService1()
        {
            //var token = _authService.GenerateJwtToken();
            var client = _clientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("http://fastapi_backend:9001/api/service1", null);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error calling service1: {response.StatusCode}");
            }
        }

        public async Task<string> CallService2()
        {
            //var token = _authService.GenerateJwtToken();
            var client = _clientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await client.GetAsync("http://fastapi_backend:9001/api/service2");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Error calling service2: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
