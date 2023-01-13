using Dev.Api.Controllers;
using Dev.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : MainController
    {
        public TestController(INotifier notifier, IUser apiUser) : base(notifier, apiUser)
        {
        }

        [HttpGet]
        public string Version()
        {
            return "Version 2.0";
        }
    }
}
