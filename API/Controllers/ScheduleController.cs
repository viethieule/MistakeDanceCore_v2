using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        public async Task<IActionResult> Create(CreateScheduleRq rq)
        {
            return Ok(await _scheduleService.RunCreate(rq));
        }
    }
}