using System;
using System.Linq;
using Asd_Extensions;
using Asd_Logging;
using Asd_Models;
using Microsoft.EntityFrameworkCore;

namespace Asd_Database
{
    public static class InitialDatabase
    {
        private const string ClassName = "InitialDatabase";

        public static void Local()
        {
            // информация для логирования
            var preLog = Guid.Empty + " | " + ClassName + "." + ExtensionProvider.GetCallerMethodName();
            try
            {
                using var context = new LocalDatabaseContext();
                // мигрируем базу данных
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
            }
        }

        public static void LocalData()
        {
            // информация для логирования
            var preLog = Guid.Empty + " | " + ClassName + "." + ExtensionProvider.GetCallerMethodName();
            try
            {
                using var context = new LocalDatabaseContext();

                var department1 = new Department { Name = "Shop" };
                context.Departments.Add(department1);
                var department2 = new Department { Name = "Logistics" };
                context.Departments.Add(department2);
                var department3 = new Department { Name = "Orders" };
                context.Departments.Add(department3);
                var department4 = new Department { Name = "Marketing" };
                context.Departments.Add(department4);
                var department5 = new Department { Name = "Warehouse" };
                context.Departments.Add(department5);

                context.SaveChanges();

                var employee1 = new Employee { Name = "Harry", Salary = 5000, DepartmentId = department1.Id };
                context.Employees.Add(employee1);
                var employee2 = new Employee { Name = "Oliver", Salary = 4500, DepartmentId = department2.Id };
                context.Employees.Add(employee2);
                var employee3 = new Employee { Name = "Jack", Salary = 7000, DepartmentId = department3.Id };
                context.Employees.Add(employee3);
                var employee4 = new Employee { Name = "Charlie", Salary = 2000, DepartmentId = department4.Id };
                context.Employees.Add(employee4);
                var employee5 = new Employee { Name = "Thomas", Salary = 3500, DepartmentId = department5.Id };
                context.Employees.Add(employee5);
                var employee6 = new Employee { Name = "Jacob", Salary = 1000, DepartmentId = department1.Id };
                context.Employees.Add(employee6);
                var employee7 = new Employee { Name = "Alfie", Salary = 6000, DepartmentId = department2.Id };
                context.Employees.Add(employee7);
                var employee8 = new Employee { Name = "Riley", Salary = 5000, DepartmentId = department3.Id };
                context.Employees.Add(employee8);
                var employee9 = new Employee { Name = "William", Salary = 4500, DepartmentId = department4.Id };
                context.Employees.Add(employee9);
                var employee10 = new Employee { Name = "James", Salary = 7000, DepartmentId = department5.Id };
                context.Employees.Add(employee10);
                var employee11 = new Employee { Name = "Amelia", Salary = 2500, DepartmentId = department1.Id };
                context.Employees.Add(employee11);
                var employee12 = new Employee { Name = "Olivia", Salary = 3000, DepartmentId = department2.Id };
                context.Employees.Add(employee12);
                var employee13 = new Employee { Name = "Jessica", Salary = 5000, DepartmentId = department3.Id };
                context.Employees.Add(employee13);
                var employee14 = new Employee { Name = "Emily", Salary = 4500, DepartmentId = department4.Id };
                context.Employees.Add(employee14);
                var employee15 = new Employee { Name = "Lily", Salary = 7000, DepartmentId = department5.Id };
                context.Employees.Add(employee15);
                var employee16 = new Employee { Name = "Ava", Salary = 5000, DepartmentId = department1.Id };
                context.Employees.Add(employee16);
                var employee17 = new Employee { Name = "Heather", Salary = 5000, DepartmentId = department2.Id };
                context.Employees.Add(employee17);
                var employee18 = new Employee { Name = "Sophie", Salary = 3500, DepartmentId = department3.Id };
                context.Employees.Add(employee18);
                var employee19 = new Employee { Name = "Mia", Salary = 3000, DepartmentId = department4.Id };
                context.Employees.Add(employee19);
                var employee20 = new Employee { Name = "Isabella", Salary = 3500, DepartmentId = department5.Id };
                context.Employees.Add(employee20);
                var employee21 = new Employee { Name = "Jack", Salary = 1000, DepartmentId = department1.Id };
                context.Employees.Add(employee21);
                var employee22 = new Employee { Name = "James", Salary = 1500, DepartmentId = department2.Id };
                context.Employees.Add(employee22);
                var employee23 = new Employee { Name = "Daniel", Salary = 5000, DepartmentId = department3.Id };
                context.Employees.Add(employee23);
                var employee24 = new Employee { Name = "Harry", Salary = 5000, DepartmentId = department4.Id };
                context.Employees.Add(employee24);
                var employee25 = new Employee { Name = "Charlie", Salary = 4500, DepartmentId = department5.Id };
                context.Employees.Add(employee25);
                var employee26 = new Employee { Name = "Ethan", Salary = 7000, DepartmentId = department1.Id };
                context.Employees.Add(employee26);
                var employee27 = new Employee { Name = "Matthew", Salary = 7000, DepartmentId = department2.Id };
                context.Employees.Add(employee27);
                var employee28 = new Employee { Name = "Ryen", Salary = 5000, DepartmentId = department3.Id };
                context.Employees.Add(employee28);
                var employee29 = new Employee { Name = "Riley", Salary = 6500, DepartmentId = department4.Id };
                context.Employees.Add(employee29);
                var employee30 = new Employee { Name = "Noah", Salary = 3500, DepartmentId = department5.Id };
                context.Employees.Add(employee30);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
            }
        }
    }
}
