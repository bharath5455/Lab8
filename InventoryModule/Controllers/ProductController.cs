using InventoryModule.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryModule.Controllers
{
    public class ProductController : Controller
    {
        string cs = "Data Source=(localdb)\\ProjectModels;Initial Catalog=InventoryDB;Integrated Security=True;Connect Timeout=30;Encrypt=True" ; 
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                String query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    });
                }
            }

            return View(products);
        }
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product p)
        {
            using(SqlConnection con = new SqlConnection(cs))
            {
                String query =@"INSERT INTO Products(ProductId, ProductName, Quantity, Price) VALUES (@ProductId, @ProductName, @Quantity, @Price)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        //Update : Display existing product
        public ActionResult Edit(int id)
        {
            Product p = new Product();
            using (SqlConnection con = new SqlConnection(cs))
            {
                String query = "SELECT * FROM Products WHERE ProductId=@ProductId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    p.ProductId = Convert.ToInt32(dr["ProductId"]);
                    p.ProductName = dr["ProductName"].ToString();
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = Convert.ToDecimal(dr["Price"]);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Index");
        }
        //Update : Save updated product
        public ActionResult edit(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                String query = "UPDATE Products SET ProductName=@ProductName, Quantity=@Quantity, Price=@Price WHERE ProductId=@ProductId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}