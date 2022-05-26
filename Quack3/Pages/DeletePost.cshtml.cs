using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class DeletePostModel : PageModel
    {
        public string postID1;
        public void OnGet(string postID)
        {
            if (Request.Cookies["isAdmin"] == "True")
            {
                postID1 = postID;
                Delete();
                Response.Redirect("/#postTitlee");
            }
            else
            {
                Response.Redirect("/");
            }
        }

        void Delete()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "DELETE FROM post where postID = @postID";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postID", postID1);
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
    }
}
