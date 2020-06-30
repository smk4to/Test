using System;
using System.Threading.Tasks;
using Asd_Database;
using Asd_Extensions;
using Asd_Logging;
using Asd_Models;

namespace Asd_Providers.Data
{
    public static partial class DataProvider
    {
        public static async Task<bool> DeleteAsync(Asd_E_Type type, Guid id, string correlationId)
        {
            // информация для логирования
            var preLog = correlationId + " | " + ClassName + "." + type + "." + ExtensionProvider.GetCallerMethodName();
            // логируем начало выполнения метода
            Logging.Trace(preLog + " START");
            // выполнение метода
            try
            {
                // логируем запрос
                Logging.Request(preLog + "(Запрос метода. Исходные данные: Id = " + id + ")");
                // создаем объект ответа
                bool response;
                // удаляем данные из базы данных
                await using (var context = new LocalDatabaseContext())
                {
                    switch (type)
                    {
                        case Asd_E_Type.Employee:
                            var employee = await context.Employees.FindAsync(id);
                            context.Employees.Remove(employee);
                            response = await context.SaveChangesAsync() > 0;
                            break;
                        case Asd_E_Type.Department:
                            var department = await context.Departments.FindAsync(id);
                            context.Departments.Remove(department);
                            response = await context.SaveChangesAsync() > 0;
                            break;
                        default:
                            response = false;
                            break;
                    }
                }
                // логируем ответ
                Logging.Response(preLog + "(Ответ метода: " + response + ")");
                // возвращаем ответ
                return response;
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
                // возвращаем ответ
                return false;
            }
            finally
            {
                // логируем окончание выполнения метода
                Logging.Trace(preLog + " END");
            }
        }
    }
}
