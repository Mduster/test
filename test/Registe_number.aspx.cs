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
    public partial class Registe_number : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string number = Request.Params["Number"];
            if (string.IsNullOrWhiteSpace(number))
            {
                Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = "请输入电话号码！" }));
            }
            else if (number.Length != 11)
            {
                Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = "输入电话号码有误，请重新输入！" }));
            }
            else
            {
                DataTable table = SQLHelper.ExecuteDataTable("select * from T_Registe");
                foreach (DataRow row in table.Rows)
                {
                    string num = row["Number"].ToString();
                    if (num == number)
                    {
                        Response.Write(JsonConvert.SerializeObject(new Information { result = false, Info = "电话号码已注册，请重新输入！" }));
                    }
                }
                Response.Write(JsonConvert.SerializeObject(new Information { result = true, Info = "电话号码可以使用" }));
            }
        }
    }
}
