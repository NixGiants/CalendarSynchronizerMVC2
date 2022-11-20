using BLL.Intrfaces;
using CalendarSynchronizerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;

namespace CalendarSynchronizerWeb.Controllers
{
    public class GoogleAuthController : Controller
    {
        //private readonly IConfigurationManagerService<GoogleAuthCreds> configurationManager;
        private readonly ISha256HelperService sha256HelperService;

        public GoogleAuthController(ISha256HelperService sha256HelperService)
        {
            this.sha256HelperService = sha256HelperService;
        }

        private const string RedirectUrl = "https://localhost:44329/GoogleOAuth/Code";
        private const string CalendarScope = "https://www.googleapis.com/auth/calendar.readonly";
        private const string PkceSessionKey = "codeVerifier";

        public IActionResult RedirectOnOAuthServer()
        {
            // PCKE.
            var codeVerifier = Guid.NewGuid().ToString();
            var codeChellange = sha256HelperService.ComputeHash(codeVerifier);

            HttpContext.Session.SetString(PkceSessionKey, codeVerifier);

            var url = GoogleOAuthService.GenerateOAuthRequestUrl(CalendarScope, RedirectUrl, codeChellange);
            return Redirect(url);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
