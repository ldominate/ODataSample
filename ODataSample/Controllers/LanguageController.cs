using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Escowt.Data;
using Escowt.Domain.Globalization;
using Microsoft.Data.OData;

namespace ODataSample.Controllers
{
	public class LanguageController : EntitySetController<Language, Guid>
    {
		EscowtDB _escowtDB;

		public LanguageController()
		{
			_escowtDB = new EscowtDB(ConfigurationManager.ConnectionStrings["ConnectionDB"].ToString());
		}

		public override IQueryable<Language> Get()
		{
			return _escowtDB.Languages;
		}

		protected override Language GetEntityByKey(Guid key)
		{
			return _escowtDB.Languages.FirstOrDefault(l => l.Guid == key);
		}

		protected override Language CreateEntity(Language language)
		{
			if (ModelState.IsValid)
			{
				language.Guid = Guid.NewGuid();
				_escowtDB.Languages.Add(language);
				_escowtDB.SaveChanges();
				return language;
			}
			else
			{
				var response = Request.CreateResponse(HttpStatusCode.BadRequest, new ODataError
				{
					ErrorCode = "ValidationError",
					Message = string.Join(";", ModelState.Values.First().Errors.Select(e => e.ErrorMessage).ToArray())

				});
				throw new HttpResponseException(response);
			}
		}

		protected override Language UpdateEntity(Guid key, Language language)
		{
			if (!_escowtDB.Languages.Any(l => l.Guid == key))
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			_escowtDB.Languages.Attach(language);
			_escowtDB.Entry(language).State = EntityState.Modified;
			_escowtDB.SaveChanges();
			return language;
		}

		protected override Language PatchEntity(Guid key, Delta<Language> patch)
		{
			var language = _escowtDB.Languages.FirstOrDefault(l => l.Guid == key);

			if (language == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			patch.Patch(language);
			_escowtDB.Languages.Attach(language);
			_escowtDB.SaveChanges();
			return language;
		}

		public override void Delete([FromODataUri]Guid key)
		{
			var language = _escowtDB.Languages.FirstOrDefault(l => l.Guid == key);

			if (language == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			_escowtDB.Languages.Remove(language);
			_escowtDB.SaveChanges();
		}

		protected override Guid GetKey(Language language)
		{
			return language.Guid;
		}

		protected override void Dispose(bool disposing)
		{
			_escowtDB.Dispose();
			base.Dispose(disposing);
		}
    }
}
