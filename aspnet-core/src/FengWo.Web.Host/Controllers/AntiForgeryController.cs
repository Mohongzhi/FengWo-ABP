using Microsoft.AspNetCore.Antiforgery;
using FengWo.Controllers;

namespace FengWo.Web.Host.Controllers
{
    public class AntiForgeryController : FengWoControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
