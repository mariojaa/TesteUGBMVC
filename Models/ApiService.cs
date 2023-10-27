//using System.Net.Http.Headers;

//namespace TesteUGBMVC.Models
//{
//    public class ApiService
//    {
//        private readonly HttpClient _httpClient;

//        public ApiService()
//        {
//            _httpClient = new HttpClient();
//            _httpClient.BaseAddress = new Uri("http://localhost:9038/api/usuario");
//            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//        }

//        public async Task<string> ObterDadosDaAPI()
//        {
//            HttpResponseMessage response = await _httpClient.GetAsync("api/usuario");
//            if (response.IsSuccessStatusCode)
//            {
//                return await response.Content.ReadAsStringAsync();
//            }
//            else
//            {
//                return null;
//            }
//        }
//    }

//}
