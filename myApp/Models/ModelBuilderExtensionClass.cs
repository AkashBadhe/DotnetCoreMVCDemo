using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.Models
{
    public static class ModelBuilderExtensionClass
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   Name = "Akash",
                   Department = Department.IT,
                   Email = "akash@gmail.com",
                   Salary = 100000
               },
               new Employee
               {
                   Id = 2,
                   Name = "Suraj",
                   Department = Department.IT,
                   Email = "suraj@gmail.com",
                   Salary = 200000
               });
        }
    }
}
