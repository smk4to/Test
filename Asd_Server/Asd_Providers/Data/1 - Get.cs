using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asd_Database;
using Asd_Extensions;
using Asd_Logging;
using Asd_Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Asd_Providers.Data
{
    public static partial class DataProvider
    {
        public static async Task<object> GetAsync(Asd_E_Type type, string correlationId)
        {
            // информация для логирования
            var preLog = correlationId + " | " + ClassName + "." + type + "." + ExtensionProvider.GetCallerMethodName();
            // логируем начало выполнения метода
            Logging.Trace(preLog + " START");
            // выполнение метода
            try
            {
                // логируем запрос
                Logging.Request(preLog + "(Запрос метода.)");
                // создаем объект ответа
                IEnumerable<object> response;
                // получаем данные из базы данных
                await using (var context = new LocalDatabaseContext())
                {
                    response = type switch
                    {
                        Asd_E_Type.Employee => await context.Employees
                            .Select(i => new
                            {
                                i.Id,
                                i.Name,
                                Department = new
                                {
                                    i.Department.Id,
                                    i.Department.Name
                                }
                            }).ToListAsync(),
                        Asd_E_Type.Department => await context.Departments
                            .Include(i => i.Employees)
                            .Select(i => new
                            {
                                i.Id,
                                i.Name,
                                i.Employees.Count,
                                Avg = i.Employees.Sum(j => j.Salary) / i.Employees.Count,
                                Employees = i.Employees
                                    .Select(j => new
                                    {
                                        j.Id,
                                        j.Name,
                                        j.Salary
                                    })
                            }).ToListAsync(),
                        _ => null
                    };
                }
                // логируем ответ
                Logging.Response(preLog + "(Ответ метода.)" + JsonConvert.SerializeObject(response, Formatting.Indented).ToTabString());
                // возвращаем ответ
                return response;
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
                // возвращаем ответ
                return null;
            }
            finally
            {
                // логируем окончание выполнения метода
                Logging.Trace(preLog + " END");
            }
        }
    }
}
