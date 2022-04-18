using DatabaseHandler.Contexts;
using DatabaseHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseHandler
{
    public class Handler
    {
        
        static void Main(string[] args)
        {
            //USED TO ADD DUMMYDATA (EMPLOYEES)

            //using VacAppContext context = new VacAppContext();
            //var employee = new Employee
            //{
            //    EmployeeFirstName = "Nalle",
            //    EmployeeLastName = "Puh"
            //};
            //context.Employees.Add(employee);
            //context.SaveChanges();

            //USED TO ADD DUMMYDATA (VACATIONAPPLICATIONS)

            //using VacAppContext context = new VacAppContext();
            //var vacApli = new VacApplication
            //{
            //    EmployeeId = 2,
            //    VacationType = "Semester",
            //    VacStartDate = new DateTime(2022, 05, 21),
            //    VacEndDate = new DateTime(2022, 06, 25),
            //    ApplicationSubmitDate = DateTime.Now
            //};
            //context.VacApplications.Add(vacApli);
            //context.SaveChanges();


        }
        public static void AddNewApplicationToDatabase(int _employeeId, string _vacationType, DateTime _vacStart, DateTime _vacEnd)
        {
            try
            {
                using VacAppContext context = new VacAppContext();
                var vacApli = new VacApplication
                {
                    EmployeeId = _employeeId,
                    VacationType = _vacationType,
                    VacStartDate = _vacStart,
                    VacEndDate = _vacEnd,
                    ApplicationSubmitDate = DateTime.Now

                };

                context.Add(vacApli);
                context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error, something went wrong with the database");
            }
        }
        public static List<Employee> LoadingAllEmployees()
        {
            try
            {
                using VacAppContext context = new VacAppContext();
                var employeesList = new List<Employee>();
                var dbEmployes = context.Employees;
                foreach (Employee employee in dbEmployes)
                {
                    employeesList.Add(employee);
                }
                return employeesList;
            }
            catch
            {
                Console.WriteLine("Error, something went wrong with the database");
                return null;
            }
        }
        public static List<VacApplication> LoadingAllApplications()
        {
            try
            {
                using VacAppContext context = new VacAppContext();
                List<VacApplication> vacApplicationsList = new List<VacApplication>();
                var vacApplications = context.VacApplications;
                foreach (VacApplication application in vacApplications)
                {
                    vacApplicationsList.Add(application);
                }
                return vacApplicationsList;
            }
            catch
            {
                Console.WriteLine("Error, something went wrong with the database");
                return null;
            }

        }
        public static List<Employee> SearchForUser(string firstname, string lastname)
        {
            try
            {
                using VacAppContext context = new VacAppContext();
                var employeesList = new List<Employee>();
                var foundUser = from employee in context.Employees
                                where employee.EmployeeFirstName.ToLower() == firstname.ToLower() && employee.EmployeeLastName.ToLower() == lastname.ToLower()
                                select employee;

                foreach (Employee employee in foundUser)
                {
                    employeesList.Add(employee);
                }
                return employeesList;
            }
            catch
            {
                Console.WriteLine("Error, something went wrong with the database");
                return null;
            }

        }
        public static void AddNewUser(Employee newEmployee)
        {
            try
            {
                using VacAppContext context = new VacAppContext();
                context.Employees.Add(newEmployee);
                context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error, something went wrong with the database");
            }
            
        }
    }
}
