using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class ModifyPostModel : PageModel
    {
        string postID1;
        public List<Post> listPost = new List<Post>();
        public void OnGet(string postID)
        {
            if (Request.Cookies["isAdmin"] == "True")
            {
                postID1 = postID;
                FetchPost();
            }
            else
            {
                Response.Redirect("/");
            }
        }

        public void OnPost(string postID)
        {
            postID1 = postID;
            Modify();
            var redir = "/blog?postID=" + postID1;
            Response.Redirect(redir);
        }

        void Modify()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "UPDATE post SET postTitle = @postTitle , postImage = @postImage, postText = @postText WHERE postID = @postID";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postTitle", Request.Form["postTitle"].ToString());
                    cmd.Parameters.AddWithValue("@postImage", Request.Form["postImage"].ToString());
                    cmd.Parameters.AddWithValue("@postText", Request.Form["postText"].ToString());
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

        void FetchPost()
        {
            listPost.Clear();
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT * FROM post WHERE postID = @postID;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postID", postID1);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            listPost.Add(new Post(sdr["postID"].ToString(), sdr["postImage"].ToString(), sdr["postTitle"].ToString(),
                                sdr["postDate"].ToString(), sdr["postText"].ToString(), int.Parse(sdr["postReach"].ToString()) + 1, sdr["isPinned"].ToString(),
                                int.Parse(sdr["userID"].ToString())));
                        }
                    }
                    con.Close();
                }
            }

        }


    }
}
