using GatePassCancellation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GatePassCancellation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2(string gatepassnumber)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                SqlCommand cmd = new SqlCommand("usp_GatePassCanecellationNoman", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GatePassNumber", gatepassnumber);
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                ViewBag.Table1 = dataSet.Tables[0];
            }
            if (TempData["message"] != null)
            {

                ViewBag.Message = TempData["message"];
            }
            return View(nameof(Index));
        }
        public IActionResult Update(string GatePassNumber, string Remarks)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateGatePassCanecellationNoman", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GatePassNumber", GatePassNumber);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Userid", HttpContext.Session.GetString("Userid"));

                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();

                if (result > 0)
                {
                    TempData["message"] = "Gate Pass Has Been Updated Successully, Gate Pass Number = (" + GatePassNumber + "), Reason for update = (" + Remarks + ")";
                    return RedirectToAction(nameof(Index2), new { gatepassnumber = GatePassNumber });
                }
            }
            return View();
        }

        public IActionResult Revert(string GatePassNumber, string Remarks)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                SqlCommand cmd = new SqlCommand("usp_RevertGatePassCanecellationNoman", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GatePassNumber", GatePassNumber);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();

                if (result > 0)
                {
                    TempData["message"] = "Gate Pass Has Been Revert Successully, Gate Pass Number = (" + GatePassNumber + "), Reason for update = (" + Remarks + ")";
                    return RedirectToAction(nameof(Index2), new { gatepassnumber = GatePassNumber });
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
