using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// This class uses the Firebase.Auth library and is based
// on the tutorial: https://www.youtube.com/watch?v=sE-RYw87_vQ

namespace Stories.Server.Controllers
{
    /// <summary>
    /// Responsible for authenticating against Firestore's database (in Firebase, see Develop | Authentication | Users).
    /// </summary>
    public class AccountController : Controller
    {
        private const string WebApiKey = "AIzaSyCZoLWI8iNWVxgfkPRD9NTqc4kOsWETW6A";
        private const string Bucket = "gs://stories-23368.appspot.com";

        /* GET: Account
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.
                if (this.Request.IsAuthenticated)
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch
            {
                throw;
            }
        }*/

        /* POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
