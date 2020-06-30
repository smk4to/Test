using System;
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
        public static async Task<bool> PutAsync<T>(Asd_E_Type type, Guid id, T request, string correlationId)
        {
            // информация для логирования
            var preLog = correlationId + " | " + ClassName + "." + type + "." + ExtensionProvider.GetCallerMethodName();
            // логируем начало выполнения метода
            Logging.Trace(preLog + " START");
            // выполнение метода
            try
            {
                // логируем запрос
                Logging.Request(preLog + "(Запрос метода. Исходные данные: Id = " + id + ")" + JsonConvert.SerializeObject(request, Formatting.Indented).ToTabString());
                // создаем объект ответа
                bool response;
                // обновляем данные в базе данных
                await using (var context = new LocalDatabaseContext())
                {
                    context.Entry(request).State = EntityState.Modified;
                    response = await context.SaveChangesAsync() > 0;
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
