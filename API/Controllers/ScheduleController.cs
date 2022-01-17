using System;
using System.Threading.Tasks;
using API.Common;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ScheduleController : BaseApiController
    {
        public ScheduleController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateScheduleRq rq)
        {
            return Ok(await this.RunAsync<CreateScheduleService, CreateScheduleRq, CreateScheduleRs>(rq));
        }
    }
}