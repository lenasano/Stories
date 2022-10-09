using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Stories.Client.Authentication
{
    /// <summary>
    /// A mock up for authenticating users and propagating user claims such as would be returned from an
    /// authentication service such as Firebase.
    /// </summary>
    public class MockupAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return await Task.FromResult(new AuthenticationState(AnonymousUser));
        }

        //Anonymous user can has no claims but requires an empty identity
        private ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));

        private ClaimsPrincipal UserJohn
        {
            get
            {
                var claims = 
                    new[] {
                        new Claim(ClaimTypes.Name, "john@stories.com"),
                        new Claim(ClaimTypes.Role, "author"),
                    };

                var identity = new ClaimsIdentity(claims, "mock_up");
                return new ClaimsPrincipal(identity);
            }
        }

        public bool MockSignIn(string username, string password)
        {
            /// Here is where we would send an API request to a Controller on the Server (within the
            /// Stories.Server project). The controller would forward the request to authenticate
            /// the user with an authentication server.

            if (username != UserJohn.Identity?.Name || password != "hi") return false;  //* this is a mock-up, but an authentication server would authenticate the user

            var result = Task.FromResult(new AuthenticationState(UserJohn));            //* then the server would send back the user's claim, containing user info

            NotifyAuthenticationStateChanged(result);
            return true;
        }

        public void MockSignOut()
        {
            var result = Task.FromResult(new AuthenticationState(AnonymousUser));
            NotifyAuthenticationStateChanged(result);
        }
    }
}
