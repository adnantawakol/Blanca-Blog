using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class myProfileModel : PageModel
    {
        public string username = "";
        public string email = "";
        public string password = "";
        public string password2 = "";
        public string userImage = "";
        void GetProfileInfo()   
        {   

            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select * from Account where userID = @uid";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@uid", Request.Cookies["userID"]);
                    /*Console.WriteLine(query);*/
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            username = sdr["username"].ToString();
                            email = sdr["email"].ToString();
                            password = sdr["password"].ToString();
                            userImage = sdr["userImage"].ToString();
                        }
                    }
                    con.Close();
                }

            }
        }
        void UpdateInfo()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "UPDATE Account set username = @username, email = @email, password = @password, userImage = @userImage where userID = @uid";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@username", Request.Form["username"].ToString());
                    cmd.Parameters.AddWithValue("@email", Request.Form["email"].ToString());
                    cmd.Parameters.AddWithValue("@password", Request.Form["password"].ToString());
                    cmd.Parameters.AddWithValue("@userImage", Request.Form["userImage"].ToString());
                    cmd.Parameters.AddWithValue("@uid", Request.Cookies["userid"].ToString());
                    /*password2 = Request.Form["password2"].ToString();
                    Console.WriteLine("password = " + password);
                    Console.WriteLine("password2 = " + password2);*/

                    /*Console.WriteLine(query);*/
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
            if (string.IsNullOrEmpty(Request.Cookies["userID"]))
            {
                Response.Redirect("/login");
            }
            else
            {
                GetProfileInfo();
                
            }
        }
        public void OnPost()
        {
            if (Request.Form["password"].ToString() == Request.Form["password2"].ToString())
            {
                UpdateInfo();
                Response.Redirect("/myProfile");
            }
            else
            {
                Response.Redirect("/myProfile");

            }
        }
    }
}
