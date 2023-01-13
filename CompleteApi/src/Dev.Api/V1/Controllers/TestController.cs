using Dev.Api.Controllers;
using Dev.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.V1.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : MainController
    {
        public TestController(INotifier notifier, IUser apiUser) : base(notifier, apiUser)
        {
        }

        [HttpGet]
        public string Version()
        {
            return "Version 1.0";
        }
    }
}
