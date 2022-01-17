using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {

        public ScheduleController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateScheduleRq rq)
        {
            return Ok();
        }
    }
}