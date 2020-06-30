using System;
using System.IO;
using Asd_Database;
using Asd_Extensions;
using Asd_LocalServer.Models;
using Asd_Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Asd_LocalServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // инициируем базу данных
            InitialDatabase.Local();

            // добавляем возможность кроссдоменных запросов
            services.AddCors(options => options.AddPolicy("AlowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            // добавляем проверку запроса и устанавливаем параметры сериализации классов в запросах и ответах
            services.AddControllers(options => { options.Filters.Add(typeof(ValidatorActionFilter)); })
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // чтобы запрос OPTIONS из Angular проходил нормально
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            // применяем возможность кроссдоменных запросов
            app.UseCors("AlowAll");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // получаем запрос
            var request = filterContext.HttpContext.Request;
            // ищем в заголовках запроса CorrelationId и запоминаем его
            if (!request.Headers.TryGetValue("CorrelationId", out var correlationIds) || correlationIds.Count != 1 || !Guid.TryParse(correlationIds[0], out var correlationId))
            {
                correlationId = Guid.NewGuid();
            }
            var normalizeCorrelationId = correlationId.ToString("D");
            // добавляем в заголовок ответа CorrelationId
            filterContext.HttpContext.Response.Headers.Add("CorrelationId", normalizeCorrelationId);
            // сохраняем CorrelationId в идентификатор запроса для использования в методах
            filterContext.HttpContext.TraceIdentifier = normalizeCorrelationId;
            // логируем данные запроса: метод, хост, адрес, заголовки, куки, тело
            try
            {
                var preLog = filterContext.HttpContext.TraceIdentifier + " | " +
                             request.Method + " " + request.Host.Value + request.Path.Value + " " +
                             filterContext.ActionDescriptor.DisplayName.ToTrimString() +
                             "Headers:".ToTabString() + JsonConvert.SerializeObject(request.Headers, Formatting.Indented).ToTabString(2) +
                             "Cookies:".ToTabString() + JsonConvert.SerializeObject(request.Cookies, Formatting.Indented).ToTabString(2) +
                             "Body:".ToTabString();


                var body = "";
                request.EnableBuffering();
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body))
                {
                    body += reader.ReadToEndAsync();
                }
                // логируем запрос
                Logging.Debug(preLog + body.ToTabString(2));
            }
            catch (Exception ex)
            {
                // логируем исключение
                Logging.Debug("Exception " + ex.Message + ex.StackTrace.ToTabString());
            }
            // если запрос корректный, то выходим из метода
            if (filterContext.ModelState.IsValid) return;
            // если запрос не корректный, то логируем его и возвращаем ответ с сообщением об ошибке
            var response = new Asd_Response(Asd_E_ResponseCode.UnknownError, null, null, normalizeCorrelationId);
            if (!filterContext.ModelState.IsValid) response = new Asd_Response(Asd_E_ResponseCode.ModelStateError, null, null, normalizeCorrelationId);
            // логируем ответ
            Logging.Error(filterContext.HttpContext.TraceIdentifier + " | " + response.Message);
            // возвращаем ответ
            filterContext.Result = new OkObjectResult(response);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
