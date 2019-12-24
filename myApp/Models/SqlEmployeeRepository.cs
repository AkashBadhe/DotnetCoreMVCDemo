using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDBContext context;

        public SqlEmployeeRepository(AppDBContext context)
        {
            this.context = context;
        }
        Employee IEmployeeRepository.AddEmployee(Employee newEmployee)
        {
            context.Employees.Add(newEmployee);
            context.SaveChanges();
            return newEmployee;
        }

        Employee IEmployeeRepository.DeleteEmployee(int id)
        {
            Employee employee = context.Employees.Find(id);
            if(employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;    
        }

        Employee IEmployeeRepository.GetEmployee(int id)
        {
            return context.Employees.Find(id);
        }

        IEnumerable<Employee> IEmployeeRepository.GetEmployees()
        {
            return context.Employees;
        }

        Employee IEmployeeRepository.UpdateEmployee(Employee employeeChanges)
        {
            var employee = context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
