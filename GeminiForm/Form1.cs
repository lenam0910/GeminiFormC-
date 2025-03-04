using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;  // Thêm thư viện Newtonsoft.Json để xử lý JSON

namespace GeminiForm
{
    public partial class Form1 : Form
    {
        private string apiKey = "";  

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string apiKeyFrom)
        {
            InitializeComponent();
            apiKey = apiKeyFrom;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string userInput = textBox1.Text;
            string output = await SendRequestAndGetResponse(userInput);

            // Xử lý xuống dòng
            output = output.Replace("\\n", Environment.NewLine)
                           .Replace("\n", Environment.NewLine)
                           .Replace("**", "");

            richTextBox1.Text = output;
        }

        private async Task<string> SendRequestAndGetResponse(string userInput)
        {
            string jsonBody = $@"{{
                ""contents"": [
                    {{
                        ""role"": ""user"",
                        ""parts"": [
                            {{
                                ""text"": ""{userInput}""
                            }}
                        ]
                    }}
                ]
            }}";

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}");
            request.Content = new StringContent(jsonBody, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request).ConfigureAwait(false);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Parse JSON bằng JObject
                    var json = JObject.Parse(responseBody);
                    var outputText = json["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                    return outputText ?? "Không nhận được phản hồi từ AI.";
                }
                catch (Exception ex)
                {
                    return $"Lỗi xử lý JSON: {ex.Message}";
                }
            }
            else
            {
                return $"Lỗi API: {response.StatusCode} - {response.ReasonPhrase}\nChi tiết: {responseBody}";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KeyForm keyForm = new KeyForm();
            keyForm.Show();
            this.Hide();
        }
    }
}
