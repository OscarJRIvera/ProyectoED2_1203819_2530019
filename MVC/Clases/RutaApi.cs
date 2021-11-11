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
            //Api.BaseAddress = new Uri("https://634b-2800-98-110f-f89-d80f-1430-8425-13ba.ngrok.io/api/");
            Api.BaseAddress = new Uri("https://localhost:44342/api/");
            Api.DefaultRequestHeaders.Clear();
            Api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaction/json"));
        }
    }
}
