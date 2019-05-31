using Newtonsoft.Json;
using System.Collections.Generic;

namespace WarframeDucats.Models
{
	public class UserOrdersResponse
	{
		[JsonProperty("sell_orders")]
		public List<UserItemOrder> Sell { get; set; }
	}
}
