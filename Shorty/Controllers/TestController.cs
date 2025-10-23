using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;

namespace Shorty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(TestRepository testRepository) : ControllerBase
    {
        [HttpPost("create-table")]
        public async Task<IActionResult> TestAction(int i)
        {
            await testRepository.TestAsync(i);
            return Ok();
        }
    }
}
