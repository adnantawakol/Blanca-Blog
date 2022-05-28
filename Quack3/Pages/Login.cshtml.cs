using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace Quack3.Pages
{
    public class LoginModel : PageModel
    {
        string userID = "";
        string isAdmin = "";
        string planID = "";
        string email = "";

        void loginQuery()
        {

            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select * from Account where Email = @email and password = @password";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@email", Request.Form["email"].ToString());
                    cmd.Parameters.AddWithValue("@password", Request.Form["password"].ToString());

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            userID = sdr["userID"].ToString();
                            isAdmin = sdr["isAdmin"].ToString();
                            planID = sdr["planID"].ToString();
                            email = sdr["email"].ToString();
                        }
                        Console.WriteLine(userID + "id in loginquery");
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
            loginQuery();
            if (userID != "")
            {
                //Add the Cookie to Browser.
                Response.Cookies.Append("userID", userID);
                Response.Cookies.Append("isAdmin", isAdmin);
                Response.Cookies.Append("planID", planID);
                Response.Cookies.Append("email", email);
                Response.Redirect("/");

            }
            else
            {
                ViewData["Message"] = string.Format("Wrong Email or Password");
            }
        }
    }
}
