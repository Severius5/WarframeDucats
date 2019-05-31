using Newtonsoft.Json;
using System.Collections.Generic;

namespace WarframeDucats.Models
{
	public class ItemsEnResponse
	{
		[JsonProperty("en")]
		public List<Item> EngItems { get; set; }
	}
}
