using giveaway_tool.Contracts;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using System.Text;

namespace giveaway_tool.Services;

public class WorkspaceCookieService(IDataProtectionProvider provider) : IWorkspaceCookieService
{

    private readonly IDataProtector _protector = provider.CreateProtector("GiveawayTool.WorkspaceCookie.v1");

    public string CreateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    public string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }

    public string Protect(string token) => _protector.Protect(token);

    public string Unprotect(string protectedToken)
    {
        try
        {
            var token = _protector.Unprotect(protectedToken);
            if (string.IsNullOrWhiteSpace(token) || token.Length > 128) return null;
            return token;
        }
        catch (CryptographicException)
        {
            return null;
        }
    }
}