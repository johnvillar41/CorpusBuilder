using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CorpusBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            InitAsync().GetAwaiter().GetResult();
        }
        private static async Task InitAsync()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Apikey", "MyUltimateSecretKeyNYAHAHAHAHAHAHAHA");
            string fileName = "corpusword_notfiltered.txt";            
            foreach (var line in File.ReadLines(fileName))
            {                
                var response = await httpClient.GetAsync($"http://192.168.1.105:105/Check/{line}");
                var result = response.IsSuccessStatusCode;
                if (result)
                {
                    var synonym = await CheckForSynonymsAsync(line, httpClient);
                    if (!response.Content.ReadAsStringAsync().Result.Equals("true") && synonym != "[]")
                    {
                        System.Console.WriteLine($"CorpusWord: {line}");
                        System.Console.WriteLine($"Synonyms: {synonym}");
                        System.Console.WriteLine("__________________________________");
                        System.Console.WriteLine();
                    }
                }
            }
        }
        private async static Task<string> CheckForSynonymsAsync(string word, HttpClient httpClient)
        {
            string query = word;            
            var response = await httpClient.GetAsync($"http://192.168.1.105:105/Synonym/{query}");
            var synonym = await response.Content.ReadAsStringAsync();
            return synonym;
        }
    }
}
