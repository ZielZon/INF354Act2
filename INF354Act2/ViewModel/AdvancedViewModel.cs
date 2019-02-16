using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INF354Act2.Models;

namespace INF354Act2.ViewModel
{
    public class AdvancedViewModel
    {
        public IEnumerable<SelectListItem> empName { get; set; }
        public int SelectedEmpID { get; set; }

        public lgemployee emps { get; set; }
        public lgcustomer cust { get; set; }
        public List<IGrouping<string, ReportRecord>> results { get; set; }
        public Dictionary<string, double> chartData { get; set; }


        public IEnumerable<SelectListItem> employeeName { get; set; }
        public List<IGrouping<string, ReportTest>> pleaseWork { get; set; }
        public int SelectedEmployeeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class ReportRecord
    {
        public string custName { get; set; }
        public int empID { get; set; }
        public string invNum { get; set; }
        public double invTotal { get; set; }
        

    }

    public class ReportTest
    {
        public string EmployeeName { get; set; }
        public double invoiceTotal { get; set; }
        public string custName { get; set; }
        public string inv_DATEIME { get; set; }


    }
}