using System;
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
        public static async Task<Guid?> PostAsync<T>(Asd_E_Type type, T request, string correlationId)
        {
            // информация для логирования
            var preLog = correlationId + " | " + ClassName + "." + type + "." + ExtensionProvider.GetCallerMethodName();
            // логируем начало выполнения метода
            Logging.Trace(preLog + " START");
            // выполнение метода
            try
            {
                // логируем запрос
                Logging.Request(preLog + "(Запрос метода. Исходные данные.)" + JsonConvert.SerializeObject(request, Formatting.Indented).ToTabString());
                // создаем объект ответа
                Guid response;
                // добавляем данные в базу данных
                await using (var context = new LocalDatabaseContext())
                {
                    switch (type)
                    {
                        case Asd_E_Type.Employee:
                            var employee = request as Employee;
                            await context.Employees.AddAsync(employee ?? throw new InvalidOperationException());
                            await context.SaveChangesAsync();
                            response = await context.Employees.Select(i => i.Id).FirstOrDefaultAsync(i => i == employee.Id);
                            break;
                        case Asd_E_Type.Department:
                            var department = request as Department;
                            await context.Departments.AddAsync(department ?? throw new InvalidOperationException());
                            await context.SaveChangesAsync();
                            response = await context.Departments.Select(i => i.Id).FirstOrDefaultAsync(i => i == department.Id);
                            break;
                        default:
                            response = Guid.Empty;
                            break;
                    }
                }
                // логируем ответ
                Logging.Response(preLog + "(Ответ метода: Id = " + response + ")");
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
