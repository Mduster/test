using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test
{
    public partial class Registe_change : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.Params["UserName"];
            string password = Request.Params["PassWord"];
            try
            {
                DataTable table = SQLHelper.ExecuteDataTable("select * from T_Registe where UserName=@username and PassWord=@Password"
                , new SqlParameter("@username", username), new SqlParameter("@password", password));
                foreach (DataRow row in table.Rows)
                {
                    string _uname = row["UseName"].ToString();
                    string _word = SQLHelper.Decrypt(row["PassWord"].ToString());
                    string _name = row["Name"].ToString();
                    string _number = row["Number"].ToString();
                    string _sex = row["Sex"].ToString();
                    string _resume = row["Resume"].ToString();
                    string _class = row["Class"].ToString();

                    string _photo = row["Photo"].ToString();
                    string _qq = row["QQ"].ToString();
                    string _society = row["Society"].ToString();
                    string _department = row["Department"].ToString();
                    if (_uname == username)
                    {
                        if (password == _word)
                        {
                            Response.Write(JsonConvert.SerializeObject(new Information
                            {
                                result = true,
                                Info =
                                    new Person { name = _name, sex = _sex, resume = _resume, Class = _class, photo = _photo, QQ = _qq, society = _society, department = _department, number = _number }
                            }));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = exception.ToString() }));
            }
        }
    }
}