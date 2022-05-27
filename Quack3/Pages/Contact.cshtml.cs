using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Quack3.Pages
{
    public class ContactModel : PageModel
    {


        public void OnGet()
        {
            if(string.IsNullOrEmpty(Request.Cookies["userID"]))
            {
                Response.Redirect("/login");
            }
            else
            {
                CheckEmail();
            }
            
        }

        public void OnPost()
        {
            if(Request.Cookies["planID"] == "2")
            {
                SendEmail(CheckEmail(), "blancaofficial2022@gmail.com", Request.Form["subject"], "Sent by: "+ CheckEmail() + "\n" + Request.Form["body"]);
                SendEmail("blancaofficial2022@gmail.com", CheckEmail(), "Email Confirmation", "Hi Dear, \n I've recieved your email and I will be getting in contact with you soon");
            }
            else
            {
                ViewData["Message"] = string.Format("You must be a premium member to contact Blanca directly ");
                /*Thread.Sleep(1000);*/
            }
            
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            string host = "smtp.gmail.com";
            int port = 587;

            MailMessage msg = new MailMessage(from, to, subject, body);
            SmtpClient smtp = new SmtpClient(host, port);

            string username = "blancaofficial2022@gmail.com";
            string password = "dxttcadnqrigwrnm";

            smtp.Credentials = new NetworkCredential(username, password);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            try
            {
                smtp.Send(msg);
            }
            catch (Exception exp)
            {
                //Log if any errors occur
                Console.WriteLine(exp.Message);
            }
        }
        public string CheckEmail()
        {
            var email="";
            string constr = "Data Source=localhost\\MSSQLSERVER2019;Initial Catalog=Blog;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select email from Account where userID = @userid";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@userid", Request.Cookies["userid"].ToString());

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {

                        while (sdr.Read())
                        {
                            email = sdr[0].ToString();
                            Console.WriteLine(email);
                        }

                    }
                    con.Close();
                }

            }
            return email;
        }
    }
}
