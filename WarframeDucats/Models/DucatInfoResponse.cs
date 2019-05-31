using Newtonsoft.Json;
using System.Collections.Generic;

namespace WarframeDucats.Models
{
	public class DucatInfoResponse
	{
		[JsonProperty("previous_hour")]
		public List<DucatInfo> PreviousHour { get; set; }
	}
}
