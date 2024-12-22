using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        string apiKey = "jpu6GkrZxx9bZC2n6vRNU5SbNdPBw0RaXwOuXlFN"; // Thay bằng API Key của bạn
        string apiUrl = "https://api.cohere.ai/generate?version=2021-11-08";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            string prompt = "cách in HelloWorld ra console C++, trả lời ngắn gọn";
            var requestData = new
            {
                model = "command-xlarge-nightly",
                prompt = prompt,
                max_tokens = 400, // Tăng giới hạn số token để lấy nội dung dài
                temperature = 0.7,
                stop_sequences = new[] { "\n\n\n" }
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var result = await response.Content.ReadAsStringAsync();

            dynamic jsonResult = JsonConvert.DeserializeObject(result);

            //Console.WriteLine(jsonResult);
            //Console.WriteLine("\n\n\n\n\n\n");

            // Kiểm tra nếu phản hồi không hợp lệ
            if (jsonResult == null || jsonResult.text == null)
            {
                Console.WriteLine("Lỗi: Không nhận được phản hồi hợp lệ từ API.");
                return;
            }

            // In toàn bộ nội dung phản hồi
            string botReply = jsonResult.text;
            Console.WriteLine("Chatbot trả lời:");
            Console.WriteLine(botReply.Trim());
        }
    }
}
