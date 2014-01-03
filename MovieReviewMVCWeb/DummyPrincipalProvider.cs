using System.Security.Principal;

namespace Piotr.BasicHttpAuth.Web
{
    public class DummyPrincipalProvider : IProvidePrincipal
    {
        private const string Username = "ZgurJ420";
        private const string Password = "73@vko1890";

        public IPrincipal CreatePrincipal(string username, string password)
        {
            if (username != Username || password != Password)
            {
                return null;
            }

            var identity = new GenericIdentity(Username);
            IPrincipal principal = new GenericPrincipal(identity, new[] { "User" });
            return principal;
        }
    }
}