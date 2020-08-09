using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace _91APP_Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult OrderList()
        {
            List<OrderInfo> orderInfos = new List<OrderInfo>();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["Northwind"].ConnectionString))
            {
                
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 300;
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM [91APP_Order] ";

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            OrderInfo order = new OrderInfo();
                            {
                                order.OrderId = dr["OrderId"].ToString();
                                order.OrderItem = dr["OrderItem"].ToString();
                                order.Price = dr["Price"].ToString();
                                order.Cost = dr["Cost"].ToString();
                                order.Status = dr["Status"].ToString();
                            }
                            orderInfos.Add(order);
                        }
                    }
                }
            }

                return View(orderInfos);
        }

        public ActionResult UpdateOrderStatus(string Data)
        {
            var js = new JavaScriptSerializer();
            var OrderList = js.Deserialize<List<string>>(Data);
            var Orderstring = "";
            foreach (var OrderId in OrderList)
            {
                Orderstring += ",'"+OrderId+"'";
            }
            var sql = string.Format("  update [91APP_Order] set Status = 'To be shipped' where OrderId in ({0}) ", Orderstring.Substring(1));

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["Northwind"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 300;
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    conn.Open();
                    //cmd.Parameters.Clear();
                    //cmd.Parameters.AddWithValue("@OrderId", Orderstring.Substring(1));
                    cmd.ExecuteNonQuery();


                }
            }



            return Content("成功!");
        }


        public class OrderInfo
        {
            public string OrderId { get; set; }
            public string OrderItem  { get; set; }
            public string Price { get; set; }
            public string Cost { get; set; }
            public string Status { get; set; }
        }

    }
}