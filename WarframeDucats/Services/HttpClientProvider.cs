using System;
using System.Net.Http;

namespace WarframeDucats.Services
{
	public static class HttpClientProvider
	{
		static HttpClientProvider()
		{
			var handler = new HttpClientHandler
			{
				AllowAutoRedirect = false
			};

			Client = new HttpClient(handler)
			{
				BaseAddress = new Uri("https://api.warframe.market/v1/")
			};
		}

		public static HttpClient Client { get; }
	}
}
