using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface IGoogleOAuthService
    {
        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge);
        public Task<TokenRes> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl);
        public Task<TokenRes> RefreshTokenAsync(string refreshToken);
    }
}
