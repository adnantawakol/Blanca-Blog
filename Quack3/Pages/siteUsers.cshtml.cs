using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Quack3.Pages
{
    public class siteUsersModel : PageModel
    {
        public List<Users> listUsers = new List<Users>();
        void FetchUsers()
        {
            listUsers.Clear();
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT * FROM account;";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            listUsers.Add(new Users(int.Parse(sdr["userID"].ToString()), sdr["username"].ToString(), sdr["email"].ToString(),
                                sdr["password"].ToString(), Convert.ToInt32(sdr["planID"].ToString()), sdr["userImage"].ToString()));
                        }
                    }
                    con.Close();
                }
            }
        }

        void DeleteAccount()
        {
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //should be needed category and government
                string query = "delete from comment where postID in (select postID from post where userID = @userID);" +
                    "delete from account where userID = @userID; ";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@userID", Request.Form["Del"].ToString());
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
            if (Request.Cookies["isAdmin"].ToString() == "False")
            {
                Response.Redirect("/");
            }
            else
            {
                FetchUsers();
            }
        }
        public void OnPost()
        {
            DeleteAccount();
            Response.Redirect("/siteUsers#del");

        }
    }
}



public class Users
{
    public int userID;
    public string username;
    public string email;
    public string password;
    /*public int isAdmin;*/
    public int planID;
    public string userImage;

    public Users(int userID, string username, string email, string password/*, int isAdmin*/
        , int planID, string userImage)
    {
        this.userID = userID;
        this.username = username;
        this.email = email;
        this.password = password;
        /*this.isAdmin = isAdmin;*/
        this.planID = planID;
        this.userImage = userImage;
        
    }
}