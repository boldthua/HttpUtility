using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HttpUtility
{
    internal class Program
    {
        public static readonly string putUrl = "https://jsonplaceholder.typicode.com/posts/1";
        public static readonly string apiUrl = "https://jsonplaceholder.typicode.com/posts";
        public static HttpRequest request = new HttpRequest(apiUrl);
        static async Task Main(string[] args)
        {
            //var articles = await request.Get<List<Article>>(apiUrl);
            //var print = JsonConvert.SerializeObject(articles);
            //Console.WriteLine(print);

            //Article article = new Article();
            //article.title = "1111";
            //article.id = 10;
            //article.userId = 1221;
            //article.body = "7788";

            //var articles = await request.Post(apiUrl, article);
            //var print = JsonConvert.SerializeObject(articles);
            //Console.WriteLine(print);
        }
    }
}
