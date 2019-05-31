using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class ItemsResponse
	{
		[JsonProperty("items")]
		public ItemsEnResponse Langs { get; set; }
	}
}
