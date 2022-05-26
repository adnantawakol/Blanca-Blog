using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class AdminModel : PageModel
    {
        public int NoUsers;
        public int NoComments;
        public int MaxCommentsPostID;
        public string MaxCommentsPostTitle;
        int CountUsers()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //should be needed category and government
                string query = "SELECT count(userID) FROM account";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            NoUsers = int.Parse(sdr[0].ToString());
                        }
                    }
                    con.Close();
                }
            }
            return NoUsers;
        }

        int CountPosts()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //should be needed category and government
                string query = "SELECT count(postID) FROM post;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            NoComments = int.Parse(sdr[0].ToString());
                        }
                    }
                    con.Close();
                }
            }
            return NoComments;
        }

        void PostMaxComments()
        {
            
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //should be needed category and government
                string query = "SELECT n.postID,p.postTitle FROM Num_comments as n JOIN post as p on n.postID = p.postID where n.Num_comments = (SELECT MAX(Num_comments) from Num_comments);";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            MaxCommentsPostID = int.Parse(sdr[0].ToString());
                            MaxCommentsPostTitle = sdr[1].ToString();
                        }
                    }
                    con.Close();
                }
            }
        }


        public void OnGet()
        {

            if (Request.Cookies["isAdmin"].ToString() == "False")
            {
                Response.Redirect("/");
            }
            else
            {
                CountUsers();
                CountPosts();
                PostMaxComments();
            }
            
        }
    }
}
