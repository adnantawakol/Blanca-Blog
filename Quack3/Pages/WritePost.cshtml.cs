using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class WritePostModel : PageModel
    {
        public List<Post> listPinnedPosts = new List<Post>();
        void SubmitPost()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO post(postTitle , postImage , postText  , userID, postDate) VALUES (@postTitle , @postImage , @postText  , @userID, CURRENT_TIMESTAMP)";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postTitle", Request.Form["postTitle"].ToString());
                    cmd.Parameters.AddWithValue("@postImage", Request.Form["postImage"].ToString());
                    cmd.Parameters.AddWithValue("@postText", Request.Form["postText"].ToString());
                    cmd.Parameters.AddWithValue("@userID", Request.Cookies["userid"].ToString());

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

        bool PostingAvailable()     //method to check if non admin premium user has not posted 2 times in the last 30 days
        {
            bool postingAvailable;
            int postsCount = 0;
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "Select count(*) from DateDifferences where DateDiff < 30 AND UserID = @userid;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@userid", Request.Cookies["userID"]);

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            postsCount = int.Parse(sdr[0].ToString());
                        }
                    }

                    con.Close();
                    if(postsCount < 2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        void FetchPinnedPosts()
        {
            listPinnedPosts.Clear();
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT * FROM post where isPinned = 1 order by postID DESC;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            listPinnedPosts.Add(new Post(sdr["postID"].ToString(), sdr["postImage"].ToString(), sdr["postTitle"].ToString(),
                                sdr["postDate"].ToString(), sdr["postText"].ToString(), int.Parse(sdr["postReach"].ToString()) + 1, sdr["isPinned"].ToString(),
                                int.Parse(sdr["userID"].ToString())));
                        }
                    }
                    con.Close();
                }
            }
        }

        public void OnGet()
        {
            if(string.IsNullOrEmpty(Request.Cookies["userID"]))
            {
                Response.Redirect("/login");
            }
            FetchPinnedPosts();
        }

        public void OnPost()
        {
            if (Request.Cookies["isAdmin"].ToString() == "true" || Request.Cookies["planID"].ToString() == "2")
            {
                if (PostingAvailable())
                {
                    SubmitPost();
                }
                else
                {
                    ViewData["Message"] = string.Format("You can't post more than two times a month");
                }
            }
            else
            {
                ViewData["Message"] = string.Format("You must be a premium member to post");
            }
        }
    }
}
