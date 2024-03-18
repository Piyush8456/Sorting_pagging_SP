using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gettingData.Models
{

    public class employeeEntityModel
    {
        public List<Employees> Employees
        {
            get; set;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public class Employees
    {
        [Key]
        public int employeeId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string mobileNumber { get; set; }

        public static implicit operator Employees(List<Employees> v)
        {
            throw new NotImplementedException();
        }
    }
}