using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myApp.Models;
using myApp.ViewModals;

namespace myApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public object EmployeeViewModal { get; private set; }

        public EmployeeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: Employee
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(_employeeRepository.GetEmployees());
        }

        // GET: Employee/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if(employee != null)
            {
                EmployeeViewModel employeeViewModel = new EmployeeViewModel()
                {
                    Title = "View model demo page",
                    Employee = employee
                };
                return View(employeeViewModel);
            } else
            {
                return View("EmployeeNotFound", id);
            }
            
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = UploadFile(model);

                    Employee newEmployee = new Employee
                    {
                        Department = model.Department,
                        Email = model.Email,
                        Name = model.Name,
                        Salary = model.Salary,
                        PhotoPath = uniqueFileName
                    };

                    _employeeRepository.AddEmployee(newEmployee);

                    return RedirectToAction("Details", new { Id = newEmployee.Id });
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private string UploadFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                
            }

            return uniqueFileName;
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath,
                Id = employee.Id,
                Name = employee.Name,
                Salary = employee.Salary
            };

            return View(employeeEditViewModel);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employee employee = _employeeRepository.GetEmployee(model.Id);

                    employee.Name = model.Name;
                    employee.Department = model.Department;
                    employee.Email = model.Email;
                    employee.Salary = model.Salary;
                    if(model.Photo != null)
                    {
                        string uniqueFileName = UploadFile(model);
                        employee.PhotoPath = uniqueFileName;
                        if (model.ExistingPhotoPath != null)
                        {
                            var existingPath = Path.Combine(hostingEnvironment.WebRootPath, "Images", model.ExistingPhotoPath);
                            System.IO.File.Delete(existingPath);
                        }
                    }
                    _employeeRepository.UpdateEmployee(employee);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }   
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}