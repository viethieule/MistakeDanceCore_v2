using System;
using System.Threading.Tasks;
using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public BaseApiController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async Task<TRs> RunAsync<T, TRq, TRs>(TRq request) 
            where TRq : BaseRequest
            where TRs : BaseResponse
            where T : BaseAppService<TRq, TRs>
        {
            return await _serviceProvider.GetRequiredService<T>().RunAsync(request);
        }
    }
}