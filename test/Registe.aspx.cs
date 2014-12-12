using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace test
{
    public partial class Registe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            DataTable table = SQLHelper.ExecuteDataTable("select * from T_Registe");
            string username = Request.Params["UserName"];
            string address = Request.Params["Address"];
            string number = Request.Params["Number"];
            string password = Request.Params["PassWord"];

            #region
            ///判断输入数据是否正确，并返回成功与否的信息。
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(address))
            {
                Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = "注册失败，请输入用户名等信息！" }));
            }
            else
            {
                password = SQLHelper.Encrypt(password);
                SQLHelper.ExecuteNonQuery("insert into T_Login (UserName,PassWord,Number,Address) values (@username,@password,@number,@address)"
                    , new SqlParameter("@username", username), new SqlParameter("@password", password), new SqlParameter("@number", number), new SqlParameter("@address", address));
                Response.Write(JsonConvert.SerializeObject(new Information { result = true, Info = "注册成功" }));
            }
            #endregion
        }
    }
}
