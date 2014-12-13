using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test
{
    public partial class Registe_address : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string number = Request.Params["Address"];
            if (string.IsNullOrWhiteSpace(number))
            {
                Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = "请输入邮箱！" }));
            }
            else
            {
                DataTable table = SQLHelper.ExecuteDataTable("select * from T_Registe");
                foreach (DataRow row in table.Rows)
                {
                    string num = row["Address"].ToString();
                    if (num == number)
                    {
                        Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = "邮箱已注册，请重新输入！" }));
                    }
                }
                Response.Write(JsonConvert.SerializeObject(new Information { result = true, Info = "邮箱可以使用" }));
            }
        }
    }
}
