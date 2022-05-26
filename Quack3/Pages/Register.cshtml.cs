using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Web;

namespace Quack3.Pages
{
    public class RegisterModel : PageModel
    {

        /*public string userID;*/
        public string username;
        public string email;
        public string password;
        /*public string isAdmin;*/
        public string planID;
        public string? userImage;

        public void registerUser()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO Account(username , email , password  , planID , userImage) VALUES (@username , @email , @password  , @planID , @userImage)";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@username", Request.Form["username"].ToString());
                    cmd.Parameters.AddWithValue("@email", Request.Form["email"].ToString());
                    cmd.Parameters.AddWithValue("@password", Request.Form["password"].ToString());
                    cmd.Parameters.AddWithValue("@planID", Request.Form["plan"].ToString());
                    cmd.Parameters.AddWithValue("@userImage", Request.Form["userImage"].ToString());

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                        }
                    }
                    con.Close();
                }
            }
        }

        public void OnGet()
        {
        }
        public void OnPost()
        {
            registerUser();
            Response.Redirect("/login");
        }
    }
}
