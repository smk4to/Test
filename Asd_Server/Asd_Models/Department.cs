using System;
using System.Collections.Generic;

namespace Asd_Models
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
