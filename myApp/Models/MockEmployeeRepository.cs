using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employees;

        public MockEmployeeRepository()
        {
            _employees = new List<Employee>(){
                new Employee()
                {
                    Id = 1,
                    Name = "Akash",
                    Salary = 100000,
                    Department = Department.IT
                },
                new Employee()
                {
                    Id = 2,
                    Name = "Suraj",
                    Salary = 200000,
                    Department = Department.Finance
                }
            };  
        }
        public Employee GetEmployee(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employees;
        }

        public Employee AddEmployee(Employee newEmployee)
        {
            newEmployee.Id = _employees.Max(e=> e.Id) + 1;
            _employees.Add(newEmployee);
            return newEmployee;
        }

        public Employee DeleteEmployee(int id)
        {
            var employee = _employees.FirstOrDefault(employee => employee.Id == id);
            if (employee != null)
            {
                _employees.Remove(employee);
            }
            return employee;
        }

        public Employee UpdateEmployee(Employee employeeChanges)
        {
            var employee = _employees.FirstOrDefault(emp => emp.Id == employeeChanges.Id);
            if(employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Salary = employeeChanges.Salary;
                employee.Department = employeeChanges.Department;
                employee.Email = employeeChanges.Email;
            }
            return employee;
        }
    }
}
