using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using Escowt.Domain.Globalization;

namespace ODataSample
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			// Раскомментируйте следующую строку кода, чтобы включить поддержку запросов для действий с типом возвращаемого значения IQueryable или IQueryable<T>.
			// Чтобы избежать обработки неожиданных или вредоносных запросов, используйте параметры проверки в QueryableAttribute, чтобы проверять входящие запросы.
			// Дополнительные сведения см. по адресу http://go.microsoft.com/fwlink/?LinkId=279712.
			config.EnableQuerySupport();

			ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();

			modelBuilder.EntitySet<Language>("Language");

			Microsoft.Data.Edm.IEdmModel model = modelBuilder.GetEdmModel();
			config.Routes.MapODataRoute("ODataRoute", "odata", model);

			// Чтобы отключить трассировку в приложении, закомментируйте или удалите следующую строку кода
			// Дополнительные сведения см. по адресу: http://www.asp.net/web-api
			config.EnableSystemDiagnosticsTracing();

		}
	}
}
