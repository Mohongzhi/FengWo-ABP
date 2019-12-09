using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using FengWo.Controllers;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;

namespace FengWo.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FengWoControllerBase
    {
        IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            return View();
        }

       
    }
}
