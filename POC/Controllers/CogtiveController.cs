using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.Configuration;
using POC.Input;
using POC.Response;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace POC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CogtiveController : ControllerBase
    {
        private HttpClient _httpClient;
        private readonly OpenAIConfig _openAIConfig;
        public CogtiveController(IOptions<OpenAIConfig> openAIConfig, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _openAIConfig = openAIConfig.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string text)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAIConfig.Key);

            var model = new ChatGPTInputModel();
            model.model = _openAIConfig.Model;
            model.messages.Add(new POC.Input.Message { role = _openAIConfig.Role, content = text });

            var requestBody = JsonSerializer.Serialize(model);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode) return NotFound();

            var result = await response.Content.ReadFromJsonAsync<ChatGPTResponseModel>();
            var promptReponse = result?.choices[0].message.content;

            return Ok(promptReponse);
        }
    }
}
