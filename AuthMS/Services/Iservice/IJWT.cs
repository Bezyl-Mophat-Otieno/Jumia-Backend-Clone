using AuthMS.Models;

namespace AuthMS.Services.Iservice
{
    public interface IJWT
    {
        string GetToken(ApplicationUser user , IEnumerable<string>Roles);
    }
}
