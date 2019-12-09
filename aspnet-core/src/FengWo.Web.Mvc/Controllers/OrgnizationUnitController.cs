using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using FengWo.Controllers;
using Microsoft.AspNetCore.Hosting;

namespace FengWo.Web.Controllers
{
    [AbpMvcAuthorize]
    public class OrgnizationUnitController : FengWoControllerBase
    {
        IHostingEnvironment _hostingEnvironment;

        public OrgnizationUnitController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
