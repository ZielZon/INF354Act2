using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INF354Act2.ViewModel;
using INF354Act2.Models;
using System.IO;
using System.Data;
using System.Data.Objects.SqlClient;


namespace INF354Act2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReportingTest()
        {
            AdvancedViewModel vm = new AdvancedViewModel();

            vm.DateFrom = new DateTime(2011, 01, 30);
            vm.DateTo = new DateTime(2011, 02, 06);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ReportingTest(AdvancedViewModel vm)
        {
            using (HardwareDBEntities db = new HardwareDBEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;


                var list = db.lginvoices.Where(pp => pp.inv_DATETIME >= vm.DateFrom && pp.inv_DATETIME <= vm.DateTo).ToList().Select(xx => new ReportTest
                {
                    inv_DATEIME = xx.inv_DATETIME.Value.ToString("yyyy-mm-dd"),
                    invoiceTotal = Convert.ToDouble(xx.inv_total),
                    custName = db.lgcustomers.Where(pp => pp.cust_code == xx.cust_code).Select(x => x.cust_fname + " " + x.cust_lname).FirstOrDefault(),
                    EmployeeName = db.lgemployees.Where(pp => pp.emp_num == xx.employee_id).Select(x => x.emp_fname).FirstOrDefault()
                });
                
                vm.pleaseWork = list.GroupBy(g => g.custName).ToList();

                vm.chartData = list.GroupBy(g => g.EmployeeName).ToDictionary(g => g.Key, g => g.Sum(v => v.invoiceTotal));
                TempData["chartData"] = vm.chartData;
                return View(vm);
            }
                
        }

        public ActionResult EmployeeChart()
        {
            var data = TempData["chartData"];

            return View(TempData["chartData"]);
        }

        [HttpGet]
        public ActionResult ReportingHistory()
        {

            AdvancedViewModel vm = new AdvancedViewModel();

            vm.empName = getEmployees(0);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ReportingHistory(AdvancedViewModel vm)
        {
            using (HardwareDBEntities db = new HardwareDBEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                vm.empName = getEmployees(vm.SelectedEmpID);

                vm.emps = db.lgemployees.Where(x => x.emp_num == vm.SelectedEmpID).FirstOrDefault();

                var list = db.lginvoices.Where(pp => pp.employee_id == vm.emps.emp_num).ToList().Select(rr => new ReportRecord
                {
                    empID = Convert.ToInt32(rr.employee_id),
                    invNum = (rr.inv_num).ToString(),
                    invTotal = Convert.ToDouble(rr.inv_total),
                    custName = db.lgcustomers.Where(pp => pp.cust_code == rr.cust_code).Select(x => x.cust_fname + " " + x.cust_lname).FirstOrDefault()
                });

                vm.results = list.GroupBy(g => g.empID.ToString()).ToList();
                TempData["chartData"] = vm.chartData;

                return View(vm);
            }
            
        }

        private SelectList getEmployees(int selected)
        {
            using (HardwareDBEntities db = new HardwareDBEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                var emps = db.lgemployees.Select(x => new SelectListItem
                {
                   
                    Value = SqlFunctions.StringConvert(x.emp_num),
                    Text = x.emp_fname
                }).ToList();

                if (selected == 0)
                    return new SelectList(emps, "Value", "Text");
                else
                    return new SelectList(emps, "Value", "Text", selected);
            }
                
        }
    }
}