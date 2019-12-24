using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.Models
{
    public interface IEmployeeRepository
    {
        public IEnumerable<Employee> GetEmployees();
        public Employee GetEmployee(int id);
        public Employee AddEmployee(Employee newEmployee);
        public Employee DeleteEmployee(int id);
        public Employee UpdateEmployee(Employee employee);
    }
}
