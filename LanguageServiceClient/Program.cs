using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageServiceClient
{
	class Program
	{
		static void DisplayLanguage(LanguageService.Language language)
		{
			Console.WriteLine("{0} {1} {2}", language.Alias, language.TitleRu, language.Guid);
		}

		static void ListAllLanguage(LanguageService.Container container)
		{
			foreach (var language in container.Language)
			{
				DisplayLanguage(language);
			}
		}

		static void Main(string[] args)
		{
			Uri uri = new Uri("http://localhost:7456/odata/");
			var container = new LanguageService.Container(uri);
			container.SendingRequest2 += (s, e) =>
			{
				Console.WriteLine("{0} {1}", e.RequestMessage.Method, e.RequestMessage.Url);
			};

			ListAllLanguage(container);
		}
	}
}
