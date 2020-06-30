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
        private const string ClassName = "DataProvider";

        public static async Task<bool> FindAsync(Asd_E_Type type, Guid id, string correlationId)
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
                // получаем данные из базы данных
                await using (var context = new LocalDatabaseContext())
                {
                    response = type switch
                    {
                        Asd_E_Type.Employee => await context.Employees.FindAsync(id) != null,
                        Asd_E_Type.Department => await context.Departments.FindAsync(id) != null,
                        _ => false
                    };
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
