using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class IndexModel : PageModel
    {
        public List<Post> listPosts = new List<Post>();
        public List<Post> listPinnedPosts = new List<Post>();


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            FetchPosts();
            FetchPinnedPosts();
        }

        void FetchPosts()
        {
            listPosts.Clear();
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT * FROM post order by postID DESC;";
                using (SqlCommand cmd = new SqlCommand(query))
                {

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            listPosts.Add(new Post(sdr["postID"].ToString(), sdr["postImage"].ToString(), sdr["postTitle"].ToString(),
                                sdr["postDate"].ToString(), sdr["postText"].ToString(), int.Parse(sdr["postReach"].ToString()) + 1, sdr["isPinned"].ToString(),
                                int.Parse(sdr["userID"].ToString())));
                        }
                    }

                    con.Close();
                }
            }
        }

        public int CountComments(string postID)
        {
            int NoComments = 0;
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //should be needed category and government
                string query = "SELECT count(commentID) FROM comment where postID = @postID;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postID", postID);
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


    }

    

}