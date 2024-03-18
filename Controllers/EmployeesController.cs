using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using gettingData.Models;

namespace gettingData.Controllers
{
    public class EmployeesController : Controller
    {
        public ActionResult Index()
        {
            var columnNames = new List<string> { "EmployeeId", "FirstName", "LastName", "MobileNumber" };
            ViewBag.ColumnNames = new SelectList(columnNames);
            return View();
        }
        [HttpGet]
        public JsonResult AjaxMethod123(int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            employeeEntityModel model = new employeeEntityModel();
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;

            List<Employees> employeeEntityList = new List<Employees>();
            string constring = ConfigurationManager.ConnectionStrings["EmployeeDbConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("getData_SP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNum", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
                    cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

                    SqlParameter totalParameter = new SqlParameter("@TotalCount", SqlDbType.Int);
                    totalParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(totalParameter);

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        employeeEntityList.Add(new Employees
                        {
                            employeeId = int.Parse(sdr["employeeId"].ToString()),
                            firstName = sdr["firstName"].ToString(),
                            lastName = sdr["lastName"].ToString(),
                            mobileNumber = sdr["mobileNumber"].ToString()
                        });
                    }
                    con.Close();
                    model.TotalCount = totalParameter.Value == DBNull.Value ? 0 : Convert.ToInt32(totalParameter.Value);
                }
            }

            model.Employees = employeeEntityList;

            return Json(new { Employees = model.Employees, TotalCount = model.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        //
    }
}
