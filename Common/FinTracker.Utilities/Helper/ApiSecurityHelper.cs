using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Client;
using Serilog;

namespace FinTracker.Utilities.ApiSecurity.Helper
{
    public static class ApiSecurityHelper
    {
        private static string? _apiToken = null;
        public static async Task<string> GetAuthApiTokenAsync(string identityServerUrl)
        {
            try
            {
                if (CheckTokenIsValid(_apiToken)) { return _apiToken; }
                else
                {
                    var client = new HttpClient();
                    var disco = await client.GetDiscoveryDocumentAsync($"{identityServerUrl}");
                    if (disco.IsError)
                    {
                        Log.Error("//-----------------------------------------------------");
                        Log.Error($"// Discovery server error {disco.Error}");
                        Log.Error("//-----------------------------------------------------");
                        return string.Empty; // TODO: -EAJ- Should really return an error message about IDSRV to user
                    }
                    var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "stokware_swagger_secureapi",
                        ClientSecret = "secret",
                        Scope = "stokware_secureapi"
                    });
                    if (tokenResponse.IsError)
                    {
                        Log.Error("//-----------------------------------------------------");
                        Log.Error($"// TokenResponse error\n\n{tokenResponse.Error}");
                        Log.Error("//-----------------------------------------------------");
                        return string.Empty; // TODO: -EAJ- Should really return an error message about IDSRV to user
                    }

                    _apiToken = tokenResponse.AccessToken ?? string.Empty;
                    return _apiToken;
                }

            }
            catch (HttpRequestException exception)
            {
                Log.Error("//-----------------------------------------------------");
                Log.Error($"// TOKEN HttpRequestException thrown {exception.Message}");
                Log.Error("//-----------------------------------------------------");
                return string.Empty; // TODO: -EAJ- Should really return an error message about IDSRV to user
            }
            catch (Exception exception)
            {
                Log.Error("//-----------------------------------------------------");
                Log.Error($"// TOKEN Exception thrown {exception.Message}");
                Log.Error("//-----------------------------------------------------");
                return string.Empty; // TODO: -EAJ- Should really return an error message about IDSRV to user
            }
        }

        private static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        private static bool CheckTokenIsValid(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) { return false; }
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;
            var now = DateTime.Now.ToUniversalTime();
            var valid = tokenDate >= now;
            return valid;
        }

    }
}
