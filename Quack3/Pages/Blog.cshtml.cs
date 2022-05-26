using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Web;

namespace Quack3.Pages
{
    public class BlogModel : PageModel

    {
        public string postID1;
        public int NoComments;
        public List<Post> listPost = new List<Post>();
        public List<Post> listPosts = new List<Post>();
        public List<Comment> listComment = new List<Comment>();



        public void OnGet(string postID)
        {
            if(!string.IsNullOrEmpty(postID) & !string.IsNullOrEmpty(Request.Cookies["userID"]))
            {
                Console.WriteLine(Request.Cookies["userID"].ToString());
                postID1 = postID;
                FetchPost();
                FetchComment();
                CountComments();
                FetchRecentPosts();
            }
            else
            {
                Response.Redirect("/login");
            }
            
        }

        public void OnPost(string postID)
        {
            postID1 = postID;
            if (string.IsNullOrEmpty(Request.Cookies["userID"]))
            {
                Response.Redirect("/login");
            }
            else
            {
                PostComment();
                Response.Redirect("/Blog?postID="+postID1+"#comments-title");
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
                                sdr["postDate"].ToString(), sdr["postText"].ToString(), int.Parse(sdr["postReach"].ToString())+1, sdr["isPinned"].ToString(),
                                int.Parse(sdr["userID"].ToString())));
                        }
                    }
                    con.Close();
                }
            }
            /*Increase post reach by one after fetching post*/
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "UPDATE post SET postReach = postReach + 1 WHERE postID = @postID";
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

        void FetchRecentPosts()
        {
            listPosts.Clear();
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT TOP 4 * FROM post order by postID DESC;";
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

            void FetchComment()
        {
            listComment.Clear();
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT a.username, a.userImage, c.commentID, c.commentText, c.commentDate FROM account as a join comment as c ON c.userID = a.userID where c.postID = @postID";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postID", postID1);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            listComment.Add(new Comment(sdr["username"].ToString(), sdr["userImage"].ToString(), sdr["commentID"].ToString(), sdr["commentText"].ToString(), sdr["commentDate"].ToString() ));
                        }
                    }
                    con.Close();
                }
            }
        }
        void PostComment()
        {
            Console.WriteLine("PostID is :" + postID1);
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO comment(commentText, commentDate, postID, userID) VALUES (@commentText , CURRENT_TIMESTAMP, @postID  , @userID)";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    Console.WriteLine("PostID isssss :" + postID1);
                    cmd.Parameters.AddWithValue("@commentText", Request.Form["commentText"].ToString());
                    cmd.Parameters.AddWithValue("@postID", postID1);
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

        int CountComments()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //should be needed category and government
                string query = "SELECT count(commentID) FROM comment where postID = @postID;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@postID", postID1);
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

    }
}





public class Post
{
    public string postID;
    public string postImage;
    public string postTitle;
    public string postDate;
    public string postText;
    public int postReach;
    public string isPinned;
    public int userID;


    public Post(string postID, string postImage, string postTitle, string postDate, string postText
        , int postReach, string isPinned, int userID)
    {
        this.postID = postID;
        this.postImage = postImage;
        this.postTitle = postTitle;
        this.postDate = postDate;
        this.postText = postText;
        this.postReach = postReach;
        this.isPinned = isPinned;
        this.userID = userID;

    }
}
/**/
public class Comment
{
    public string username;
    public string userImage;
    public string commentID;
    public string commentText;
    public string commentDate;

    public Comment(string username, string userImage, string commentID, string commentText, string commentDate)
    {
        this.username = username;
        this.userImage = userImage;
        this.commentID = commentID;
        this.commentText = commentText;
        this.commentDate = commentDate;
    }
}