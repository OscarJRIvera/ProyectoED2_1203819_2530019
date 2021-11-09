using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace MVC.Clases
{
    public class RutaApi
    {
        public static HttpClient Api = new HttpClient();    
        static RutaApi()
        {
            Api.BaseAddress = new Uri("https://localhost:44342/api/");
            Api.DefaultRequestHeaders.Clear();
            Api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaction/json"));
        }
    }
}
