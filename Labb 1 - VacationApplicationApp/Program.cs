using System;
using System.Collections.Generic;
using DatabaseHandler.Models;
using System.Linq;

namespace Labb_1___VacationApplicationApp
{
    internal class Program
    {
        static private string userFirstname;
        static private string userLastname;
        static private Employee loggedonUser;

        static private bool loggedin = false;
        static private bool options = true;
        static private bool adminMenu;
        static private bool closingApp;

        static private List<Employee> employeesList = new List<Employee>();
        static private List<VacApplication> vacApplicationsList = new List<VacApplication>();
        static void Main(string[] args)
        {

            StartUp();
            do
            {
                do
                {
                    Console.Clear();
                    Login();

                } while (loggedin == false);
                if(closingApp != true)
                {
                    if (!adminMenu) Menu();
                    else AdminMenu();
                }
            } while (closingApp == false);
        }
        static void StartUp() // Used to Load all data from the database
        {
            try
            {
                Console.WriteLine("Loading data, please wait...");
                employeesList = DatabaseHandler.Handler.LoadingAllEmployees();
                vacApplicationsList = DatabaseHandler.Handler.LoadingAllApplications();
            }
            catch
            {
                Console.WriteLine("Error, something went wrong");
            }
        }
        static void Login() // first menu containing Login, admin Login and register new user.
        {
            try
            {
                Console.WriteLine("Please Choose one option: \n1. Login\n2. Register new employee\n3. Admin Meny\n4. Exit Program");
                string userInputString = Console.ReadLine();
                int userInput = int.Parse(userInputString);

                switch (userInput)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Login --- Please Enter first- and lastname with a space inbetween: ");
                        var splitname = Console.ReadLine();
                        while (!splitname.Contains(" "))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter your first and your lastname with a space inbetween");
                            splitname = Console.ReadLine();
                            Console.ResetColor();
                        }

                        userFirstname = splitname.Split(' ')[0];
                        userLastname = splitname.Split(' ')[1];
                        List<Employee> loginUser = DatabaseHandler.Handler.SearchForUser(userFirstname, userLastname);

                        if (loginUser.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("This user does not exist!");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Did you enter yout username in the right order? Firstname Lastname ( ex. Inga Berg)?");
                            Console.ResetColor();
                            Console.WriteLine("\n\nPress any key to go back to the login menu!");
                            Console.ReadLine();

                        }
                        if (loginUser.Count != 0)
                        {
                            loggedonUser = loginUser[0];
                            loggedin = true;
                            Console.Clear();
                        }
                        break;
                    case 2:
                        CreateNewUser();
                        break;
                    case 3:
                        AdminMenu();
                        break;
                    case 4:
                        Console.WriteLine("Exiting Program, Have a nice day");
                        loggedin = true;
                        closingApp = true;
                        break;
                }
            }catch
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR! Something went wrong, please try again");
                Console.ReadLine();
                Console.ResetColor();
            }
        }
        static void CreateNewUser() // Used to create a new user and add him/her to the database, also checks if user already exist
        {
            Console.Clear();
            Console.WriteLine("Creating new user\n\n");
            Console.WriteLine("Please enter your firstname: ");
            userFirstname = Console.ReadLine();
            Console.WriteLine("Please enter your lastname: ");
            userLastname = Console.ReadLine();
            Console.Clear();

            Console.ForegroundColor= ConsoleColor.Red;
            Console.WriteLine("Is this correct? Y/N");
            Console.ResetColor();
            Console.WriteLine("New user: " + userFirstname + " " + userLastname);
            string answer = Console.ReadLine().ToLower();

            while(!(answer == "y" || answer == "n"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("please enter y or n to continue");
                Console.ResetColor();
                answer = Console.ReadLine();
            }

            if (answer == "y")
            {
                List<Employee> loginUser = DatabaseHandler.Handler.SearchForUser(userFirstname, userLastname);

                if (loginUser.Count != 0)
                {
                    Console.WriteLine("ERROR, this user already exist!");
                    Console.ReadLine();
                }

                if (loginUser.Count == 0)
                {
                    var newUser = new Employee
                    {
                        EmployeeFirstName = userFirstname,
                        EmployeeLastName = userLastname
                    };
                    DatabaseHandler.Handler.AddNewUser(newUser);
                    Console.WriteLine("User successfully added, Login in...");

                    loginUser = DatabaseHandler.Handler.SearchForUser(userFirstname, userLastname);
                    loggedonUser = loginUser[0];
                    loggedin = true;
                    Console.Clear();
                }
            }
            if(answer == "n")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine("Canceling new user creation, back to login...");
                Console.ResetColor();
            }
        }
        static void Menu() // This is the main menu
        {
            do
            {
                Console.WriteLine("Logged in as: " + userFirstname + " " + userLastname + "\n\nChoose an Option: \n1. Creata a vacationapplication. \n2. See all vacationapplications.\n3. Search vacationapplication by first- or lastname. \n4. Personal Vacation application history\n5. Logout");
                string choicestring = Console.ReadLine();
                int choice = int.Parse(choicestring);


                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        AddNewVacApplication();
                        break;
                    case 2:
                        Console.Clear();
                        ShowAllVacApplications();
                        break;
                    case 3:
                        Console.Clear();
                        SearchingApplication();
                        break;
                    case 4:
                        Console.Clear();
                        ShowPersonalVacApplications();
                        break;
                    case 5:
                        Console.WriteLine("Logged out");
                        loggedin = false;
                        loggedonUser = null;
                        options = false;
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter one of the given options");
                        Console.ResetColor();
                        break;
                }
                
            } while (options == true);

        }
        static void AddNewVacApplication() // Used to create and add a new application to the database
        {
            Console.WriteLine("Please enter your vacation reason ex.(parental leave, maternity leave, vacation, off duty)");
            string vacationType = Console.ReadLine();
            Console.WriteLine("Please enter the date you wish to start your vacation(Year/Month/Day)");
            string splitStartdate = Console.ReadLine();

            DateTime startDate = DateParser(splitStartdate);

            Console.WriteLine("Please enter the date you wish to end your vacation(Year/Month/Day)");
            string splitEndDate = Console.ReadLine();

            DateTime endDate = DateParser(splitEndDate);

            Console.Clear();
            Console.WriteLine("Please Check that all the information is correct: \n" +
                "\n\tName: " + userFirstname + " " + userLastname +
                "\n\tVacationtype: " + vacationType +
                "\n\tStart of vacation: " + startDate +
                "\n\tEnd of vacation: " + endDate +
                "\n\nIs this information correct? (Y/N)");
            bool isright = false;
            do
            {
                string choice = Console.ReadLine();
                if (choice.ToLower() == "y")
                {
                    Console.WriteLine("Submitting application, please wait...");
                    DatabaseHandler.Handler.AddNewApplicationToDatabase(loggedonUser.EmployeeId, vacationType, startDate, endDate);
                    Console.WriteLine("Application successfully submited, press any key to go back to the menu!");
                    Console.ReadLine();
                    Console.Clear();
                    isright = true;
                    break;
                }
                if (choice.ToLower() == "n")
                {
                    Console.WriteLine("Canceling application");
                    Console.WriteLine("Press any key to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();
                    isright = true;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please Enter Y or N or User");
                    Console.ResetColor();
                }

            } while (isright == false);

        }
        static DateTime DateParser(string inputdate) // used to convert a string into a datetime
        {
            while (!inputdate.Contains("/"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter your the date in the given format: year/month/day (2022/01/15)");
                inputdate = Console.ReadLine();
                Console.ResetColor();
            }

            int year = int.Parse(inputdate.Split('/')[0]);
            int month = int.Parse(inputdate.Split('/')[1]);
            int day = int.Parse(inputdate.Split('/')[2]);

            if (year < 100) year = year + 2000;
            DateTime returnDate = new DateTime(year, month, day);
            return returnDate;
        }

        static void ShowAllVacApplications() // Used to show all made applications
        {
            DisplayVacApplicationsWithList(vacApplicationsList);
            Console.ReadLine();
            Console.Clear();
        }
        static void ShowPersonalVacApplications() // Used to only show the vacationapplication history of the user that is logged in 
        {
            DisplayVacApplications(loggedonUser);
        }
        static void SearchingApplication() // Used to search for the vacationapplication history of a specific user (either by first- last- or fullname)
        {
            List<Employee> searchedEmployee = new List<Employee>(); 

            Console.WriteLine("Enter First- or Lastname or both");
            string userInput = Console.ReadLine();
            if(userInput.Contains(" ")){

                string firstName = userInput.Split(' ')[0];
                string lastName = userInput.Split(' ')[1];

                var searchUser = from employee in employeesList
                                 where employee.EmployeeFirstName.ToLower() == firstName.ToLower() && employee.EmployeeLastName.ToLower() == lastName.ToLower()
                                 select employee;

                foreach(var searchresult in searchUser) searchedEmployee.Add(searchresult);
               
                if(searchedEmployee.Count == 0) Console.WriteLine("--- No Results ---");
                else if(searchedEmployee.Count != 0) DisplayVacApplications(searchedEmployee[0]);
            }
            else
            {
                var searchUser = from employee in employeesList
                                 where employee.EmployeeFirstName.ToLower() == userInput.ToLower() || employee.EmployeeLastName.ToLower() == userInput.ToLower()
                                 select employee;

                foreach (var searchresult in searchUser) searchedEmployee.Add(searchresult);

                if (searchedEmployee.Count == 0) Console.WriteLine("--- No Results ---");
                else if (searchedEmployee.Count != 0) DisplayVacApplications(searchedEmployee[0]);
            }

        }
        static void AdminMenu() // This is the menu for the admin
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Choose an Option: \n1. See all vacationapplications. \n2. See all vacationapplications for a specific month.\n3. Search vacationapplication by first- or lastname. \n4. Logout");
                string choicestring = Console.ReadLine();
                int choice = int.Parse(choicestring);

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        ShowAllVacApplications();
                        break;
                    case 2:
                        Console.Clear();
                        AdminMonthResults();
                        break;
                    case 3:
                        Console.Clear();
                        SearchingApplication();
                        break;
                    case 4:
                        Console.WriteLine("Logged out");
                        options = false;
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter one of the given options");
                        Console.ResetColor();
                        break;
                }
            } while (options == true);
        
        }
        static List<VacApplication> GetMonthlyVacApplication(int month) // returns all vacationapplication from a specific month
        {
            List<VacApplication> result = new List<VacApplication>();

            var searchQuery = from application in vacApplicationsList
                              where application.ApplicationSubmitDate.Month == month
                              select application;

            if (searchQuery != null)
            {
                foreach (VacApplication application in searchQuery)
                {
                    result.Add(application);
                }
            }

            return result;
        }
        static void AdminMonthResults() // Gets month input and returns all vacationapplications from that specific month with the help of GetMonthlyVacApplication
        {
            List<VacApplication> monthlyVacApplications = new List<VacApplication>();

            Console.WriteLine("Please enter the month (in numbers) that you want to see the applications from (ex. 04 for april)");
            string choicestring = Console.ReadLine();
            int choice = int.Parse(choicestring);

            Console.Clear();
            switch (choice)
            {
                case 1:
                    Console.WriteLine("\tJanuary");
                    monthlyVacApplications  = GetMonthlyVacApplication(1);
                    break;
                case 2:
                    Console.WriteLine("\tFebruary");
                    monthlyVacApplications = GetMonthlyVacApplication(2);
                    break;
                case 3:
                    Console.WriteLine("\tMarch");
                    monthlyVacApplications = GetMonthlyVacApplication(3);
                    break;
                case 4:
                    Console.WriteLine("\tApril");
                    monthlyVacApplications = GetMonthlyVacApplication(4);
                    break;
                case 5:
                    Console.WriteLine("\tMay");
                    monthlyVacApplications = GetMonthlyVacApplication(5);
                    break;
                case 6:
                    Console.WriteLine("\tJuni");
                    monthlyVacApplications = GetMonthlyVacApplication(6);
                    break;
                case 7:
                    Console.WriteLine("\tJuli");
                    monthlyVacApplications = GetMonthlyVacApplication(7);
                    break;
                case 8:
                    Console.WriteLine("\tAugust");
                    monthlyVacApplications = GetMonthlyVacApplication(8);
                    break;
                case 9:
                    Console.WriteLine("\tSeptember");
                    monthlyVacApplications = GetMonthlyVacApplication(9);
                    break;
                case 10:
                    Console.WriteLine("\tOctober");
                    monthlyVacApplications = GetMonthlyVacApplication(10);
                    break;
                case 11:
                    Console.WriteLine("\tNovember");
                    monthlyVacApplications = GetMonthlyVacApplication(11);
                    break;
                case 12:
                    Console.WriteLine("\tDecember");
                    monthlyVacApplications = GetMonthlyVacApplication(12);
                    break;
                default:
                    Console.WriteLine("The number is not in range");
                    break;

            }
            if(monthlyVacApplications.Count != 0) DisplayVacApplicationsWithList(monthlyVacApplications);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\tNo Results");
                Console.ResetColor();
            }
            Console.ReadLine();      
        }
        static void DisplayVacApplications(Employee employee)
        {
            var searchQuery = from application in vacApplicationsList
                              where application.EmployeeId == employee.EmployeeId
                              select application;

            foreach (var searchresult in searchQuery)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\t" + employee.EmployeeFirstName + " " + employee.EmployeeLastName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t" + searchresult.VacationType);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t" + searchresult.VacStartDate + " ---- " + searchresult.VacEndDate);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tSubmited: " + searchresult.ApplicationSubmitDate + "\n");
                Console.ResetColor();
            }
        } // Displays all Vacationapplications with a specific employees as parameter
        static void DisplayVacApplicationsWithList(List<VacApplication> vacApplications)
        {
            foreach (var application in vacApplications)
            {
                List<Employee> employees = new List<Employee>();
                var searchUser = from employee in employeesList
                                 where employee.EmployeeId == application.EmployeeId
                                 select employee;
                foreach (var employee in searchUser)
                {
                    employees.Add(employee);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\t" + employees[0].EmployeeFirstName + " " + employees[0].EmployeeLastName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t" + application.VacationType);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t" + application.VacStartDate + " ---- " + application.VacEndDate);
                Console.WriteLine("\tLength of vacation: " + (application.VacEndDate - application.VacStartDate).TotalDays);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tSubmited: " + application.ApplicationSubmitDate + "\n");
                Console.ResetColor();
            }
        }// Displays all vacationapplications from a specfic List

    }
}
