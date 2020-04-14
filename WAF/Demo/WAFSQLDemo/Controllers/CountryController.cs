using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WAFSQLDemo.Models;

namespace WAFSQLDemo.Controllers
{
    public class CountryController : Controller
    {
        static readonly string connectingString = "Server=sqlsrvwaf.database.windows.net,1433;" +
        "Initial Catalog=sqlwafdemo;Persist Security Info=False;" +
        "User ID=rezaadmin;Password=Kluwer2006!@#;MultipleActiveResultSets=False;Encrypt=True" +
        ";TrustServerCertificate=True;Connection Timeout=30;";

        // GET: Country
        public ActionResult Index()
        {
            var capitals = new List<CountryInfo>();

            using (var sqlConnection = new SqlConnection(connectingString))
            {
                var sqlCommand = new SqlCommand("SELECT Country, Capital FROM CountryInfo", sqlConnection);

                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    capitals.Add(new CountryInfo
                    {
                        Country = reader["Country"].ToString(),
                        Capital = reader["Capital"].ToString()
                    }
                    );
                }

                sqlConnection.Close();
            }

            return View(capitals);
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Country/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            using (var sqlConnection = new SqlConnection(connectingString))
            {
                var insertCommand = $"INSERT INTO CountryInfo (Country, Capital) " +
                    $"VALUES ('{collection["Country"]}', '{collection["Capital"]}')";
                var sqlCommand = new SqlCommand(insertCommand, sqlConnection);

                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }

            return RedirectToAction("Index");
        }
    }
}
