using Core.Models;

namespace BLL.Intrfaces
{
    public interface IGoogleOAuthService
    {
        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge);
        public Task<TokenRes> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl);
        public Task<TokenRes> RefreshTokenAsync(string refreshToken);
    }
}
