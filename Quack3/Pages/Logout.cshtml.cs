using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Quack3.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            Response.Cookies.Append("userID", "");
            Response.Cookies.Append("isAdmin","False" );
            Response.Cookies.Append("planID", "");
            Response.Cookies.Append("email", "");
            Response.Redirect("/");

        }
    }
}
