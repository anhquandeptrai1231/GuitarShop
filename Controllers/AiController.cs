using GuitarShop.Models;
using GuitarShop.Services.Implementations;
using GuitarShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuitarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IGeminiServices _gemini;
        public AiController(IGeminiServices gemini)
        {
            _gemini = gemini;
        }
        public record ChatRequest(List<ChatMessage> History);

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest req)
        {
            var reply = await _gemini.ChatWithContextAsync(req.History);
            return Ok(new { response = reply });
        }
    }
}
