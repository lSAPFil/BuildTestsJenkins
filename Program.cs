using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace BuildTestsJenkins
{
    class Program
    {
        public static HttpWebResponse httpResponse;
        public static string url = "http://localhost:8080/job/PetStoreAutotests/buildWithParameters?token=11886940bc6d280c88a8063584c74c9015&Tags=";
        public static string cred = Convert.ToBase64String(Encoding.ASCII.GetBytes("nkarban:Natali46571"));

        static void Main(string[] args)
        {
           // Изменяю кодировку для избежания проблем во время логирования
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Введите теги автотестов для запуска\nДоступные:\n- all,\n- any\n");
            string tag = Console.ReadLine();

            // Получения Crumble от дженкинса для последующей отправки GET запроса на Run сборки
            var crumb = GetCrumb();

            // Формирование Url запроса для запуска автотестов с определенным тегом
            Uri uri = new Uri(url + tag);

            // Создание url для отправки
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";

            // Авторизация пользователя в Jenkins и добавление Crumble
            httpRequest.Headers.Add($"Authorization: Basic {cred}");
            httpRequest.Headers.Add($"Jenkins-Crumb: {crumb}");

            Console.WriteLine(new StreamReader(httpRequest.GetResponse().GetResponseStream()).ReadToEnd());
        }

        // Отправка запроса для получения Crumblez
        public static string GetCrumb()
        {
            Uri uri = new Uri("http://localhost:8080/crumbIssuer/api/json");
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";
           
            httpRequest.Headers.Add("Authorization", $"Basic {cred}");

            var response = httpRequest.GetResponse();
            var json = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
          
            return json["crumb"].ToString();
        }
    }
}
