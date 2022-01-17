using System;
using System.Threading.Tasks;
using API.Common;
using Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SessionController : BaseApiController
    {
        public SessionController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Get(GetSessionsRq rq)
        {
            return Ok(await this.RunAsync<GetSessionsService, GetSessionsRq, GetSessionsRs>(rq));
        }
    }
}