using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using FengWo.Controllers;

namespace FengWo.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : FengWoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
