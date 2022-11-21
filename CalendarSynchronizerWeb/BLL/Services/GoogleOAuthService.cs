using BLL.Helpers;
using BLL.Intrfaces;
using Core.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GoogleOAuthService : IGoogleOAuthService
    {
        private const string _clientId = "516669132332-ril060aftkfjnc8gq5mqurffc3t95n8q.apps.googleusercontent.com";
        private const string _clientSecret = "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";

        private const string _oAuthServerEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string _tokenServerEndpoint = "https://oauth2.googleapis.com/token";

        public async Task<TokenRes> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl)
        {
            var authParams = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "code", code },
                { "code_verifier", codeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            };

            var TokenRes = await HttpClientHelper.SendPostRequest<TokenRes>(_tokenServerEndpoint, authParams);
            return TokenRes;
        }

        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"client_id", _clientId},
                { "redirect_uri", redirectUrl },
                { "response_type", "code" },
                { "scope", scope },
                { "code_challenge", codeChallenge },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };

            var url = QueryHelpers.AddQueryString(_oAuthServerEndpoint, queryParams);
            return url;
        }

        public async Task<TokenRes> RefreshTokenAsync(string refreshToken)
        {
            var refreshParams = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            var TokenRes = await HttpClientHelper.SendPostRequest<TokenRes>(_tokenServerEndpoint, refreshParams);

            return TokenRes;
        }
    }
}
