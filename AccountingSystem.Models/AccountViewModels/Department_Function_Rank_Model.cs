using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Models.AccountViewModels
{
    public class Department_Function_Rank_Model
    {
        public int Id { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string RankName { get; set; }
        public string FunctionName { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
    }
}
