using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebNorthwindOrders.database_Access_Layer
{
   
    public class db
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        public int Login(string txtUserId,string txtPassword) {
           // int res=0;
           // SqlCommand com = new SqlCommand("LoginUser",con);
            // com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@UserId",txtUserId.ToString());
            //com.Parameters.AddWithValue("@Password",txtPassword.ToString());
            //return 1;
            using (SqlCommand cmd = new SqlCommand("dbo.LoginUser",con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //MessageBox.Show(txtUserID.Text);
                // string x = "";

                cmd.Parameters.Add(new SqlParameter("@UserId", txtUserId.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Password", txtPassword.ToString()));
                con.Open();
                cmd.Connection = con;

                // DataTable DtUserPrivillege = cmd.EndExecuteReader;

                // cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);
                    //    dataGridView1.DataSource = dt;
                    if (dt.Rows.Count == 0)
                    {
                        // MessageBox.Show("Login account doesn't exist!");
                        return 0;
                    }
                    else
                    {   
                        return 1;
                    }
                    //MessageBox.Show(dt.);
                    //gvUserPrivilegeList.DataSource = dt;
                    //   gvUserPrivilegeList.DataBind();
                }

            }

        }

    }

}