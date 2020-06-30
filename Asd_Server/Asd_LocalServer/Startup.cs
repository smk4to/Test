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
            // ���������� ���� ������
            InitialDatabase.Local();

            // ��������� ����������� ������������� ��������
            services.AddCors(options => options.AddPolicy("AlowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            // ��������� �������� ������� � ������������� ��������� ������������ ������� � �������� � �������
            services.AddControllers(options => { options.Filters.Add(typeof(ValidatorActionFilter)); })
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ����� ������ OPTIONS �� Angular �������� ���������
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            // ��������� ����������� ������������� ��������
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
            // �������� ������
            var request = filterContext.HttpContext.Request;
            // ���� � ���������� ������� CorrelationId � ���������� ���
            if (!request.Headers.TryGetValue("CorrelationId", out var correlationIds) || correlationIds.Count != 1 || !Guid.TryParse(correlationIds[0], out var correlationId))
            {
                correlationId = Guid.NewGuid();
            }
            var normalizeCorrelationId = correlationId.ToString("D");
            // ��������� � ��������� ������ CorrelationId
            filterContext.HttpContext.Response.Headers.Add("CorrelationId", normalizeCorrelationId);
            // ��������� CorrelationId � ������������� ������� ��� ������������� � �������
            filterContext.HttpContext.TraceIdentifier = normalizeCorrelationId;
            // �������� ������ �������: �����, ����, �����, ���������, ����, ����
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
                // �������� ������
                Logging.Debug(preLog + body.ToTabString(2));
            }
            catch (Exception ex)
            {
                // �������� ����������
                Logging.Debug("Exception " + ex.Message + ex.StackTrace.ToTabString());
            }
            // ���� ������ ����������, �� ������� �� ������
            if (filterContext.ModelState.IsValid) return;
            // ���� ������ �� ����������, �� �������� ��� � ���������� ����� � ���������� �� ������
            var response = new Asd_Response(Asd_E_ResponseCode.UnknownError, null, null, normalizeCorrelationId);
            if (!filterContext.ModelState.IsValid) response = new Asd_Response(Asd_E_ResponseCode.ModelStateError, null, null, normalizeCorrelationId);
            // �������� �����
            Logging.Error(filterContext.HttpContext.TraceIdentifier + " | " + response.Message);
            // ���������� �����
            filterContext.Result = new OkObjectResult(response);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
