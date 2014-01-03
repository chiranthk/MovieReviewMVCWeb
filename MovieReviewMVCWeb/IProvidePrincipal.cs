using System.Security.Principal;

namespace Piotr.BasicHttpAuth.Web
{
    public interface IProvidePrincipal
    {
        IPrincipal CreatePrincipal(string username, string password);
    }
}