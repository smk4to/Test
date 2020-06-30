using System;
using System.Threading.Tasks;
using Asd_Extensions;
using Asd_LocalServer.Models;
using Asd_Logging;
using Asd_Models;
using Asd_Providers.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Asd_LocalServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : Controller
    {
        private const string ClassName = "Departments";

        [HttpGet]
        public async Task<ActionResult<object>> Get()
        {
            var correlationId = HttpContext.TraceIdentifier;
            // информация для логирования
            var preLog = correlationId + " | " + ClassName + "Controller." + ExtensionProvider.GetCallerMethodName();
            // логируем начало выполнения метода
            Logging.Trace(preLog + " START");
            // выполнение метода
            try
            {
                // логируем запрос
                Logging.Request(preLog + "(Запрос метода.)");
                // получаем данные из базы данных
                var data = await DataProvider.GetAsync(Asd_E_Type.Department, correlationId);
                // логируем ответ
                Logging.Response(preLog + "(Ответ метода.)" + JsonConvert.SerializeObject(data, Formatting.Indented).ToTabString());
                // возвращаем ответ
                var code = data == null ? Asd_E_ResponseCode.GetError : Asd_E_ResponseCode.Success;
                return new Asd_Response(code, null, data, correlationId);
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Exception(preLog + "(" + ex.Message + ")" + ex.StackTrace.ToTabString());
                // возвращаем ответ
                return new Asd_Response(Asd_E_ResponseCode.GetException, ex.Message, null, correlationId);
            }
            finally
            {
                // логируем окончание выполнения метода
                Logging.Trace(preLog + " END");
            }
        }
    }
}