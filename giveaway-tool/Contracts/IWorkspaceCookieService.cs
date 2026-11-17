namespace giveaway_tool.Contracts;
public interface IWorkspaceCookieService
{
    string CreateToken();
    string HashToken(string token);
    string Protect(string token);
    string Unprotect(string protectedToken);
}
