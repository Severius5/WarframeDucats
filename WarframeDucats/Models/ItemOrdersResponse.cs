using Newtonsoft.Json;
using System.Collections.Generic;

namespace WarframeDucats.Models
{
	public class ItemOrdersResponse
	{
		[JsonProperty("orders")]
		public List<ItemOrder> Orders { get; set; }
	}
}
