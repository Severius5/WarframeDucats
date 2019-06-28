using Newtonsoft.Json;
using System.Collections.Generic;

namespace WarframeDucats.Models
{
	public class ItemsResponse
	{
		[JsonProperty("items")]
		public List<Item> Items { get; set; }
	}
}
