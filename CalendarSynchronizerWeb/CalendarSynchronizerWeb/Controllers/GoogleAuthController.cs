using BLL.Intrfaces;
using BLL.Services;
using CalendarSynchronizerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace CalendarSynchronizerWeb.Controllers
{
    public class GoogleAuthController : Controller
    {
        //private readonly IConfigurationManagerService<GoogleAuthCreds> configurationManager;
        private readonly ISha256HelperService sha256HelperService;
        private readonly IGoogleOAuthService googleOAuthService;

        public GoogleAuthController(ISha256HelperService sha256HelperService, IGoogleOAuthService googleOAuthService)
        {
            this.sha256HelperService = sha256HelperService;
            this.googleOAuthService = googleOAuthService;
        }

        private const string RedirectUrl = "https://localhost:7060/GoogleAuth/Code";
        private const string CalendarScope = "https://www.googleapis.com/auth/calendar.readonly";
        private const string PkceSessionKey = "codeVerifier";   

        public IActionResult RedirectOnOAuthServer()
        {
            // PCKE.
            var codeVerifier = Guid.NewGuid().ToString();
            var codeChellange = sha256HelperService.ComputeHash(codeVerifier);

            HttpContext.Session.SetString(PkceSessionKey, codeVerifier);

            var url = googleOAuthService.GenerateOAuthRequestUrl(CalendarScope, RedirectUrl, codeChellange);
            return Redirect(url);
        }

        public async Task<IActionResult> CodeAsync(string code)
        {
            try
            {
                var codeVerifier = HttpContext.Session.GetString(PkceSessionKey);

                var tokenResult = await googleOAuthService.ExchangeCodeOnTokenAsync(code, codeVerifier, RedirectUrl);

                //configurationManager.ChangeAppSettingValue(opt =>
                //{
                //    opt.RefreshToken = tokenResult.RefreshToken;
                //    opt.AccessToken = tokenResult.AccessToken;
                //});
                return RedirectToAction("Privacy", "Home");

               // return RedirectToAction("IncrementalSync", "GoogleCalendar");

            }
            catch (Exception ex)
            {
                throw ex;
                //TODO Error hadling logic
            }
        }


            public IActionResult Index()
        {
            return View();
        }
    }
}
