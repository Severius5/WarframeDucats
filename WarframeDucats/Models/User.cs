using Newtonsoft.Json;

namespace WarframeDucats.Models
{
	public class User
	{
		[JsonProperty("ingame_name")]
		public string Name { get; set; }

		public Status Status { get; set; }
	}
}
