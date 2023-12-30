using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
	public class testEF
	{
		public int ID { get; set; }
		[Required]

		public string Name { get; set; } = string.Empty;

		public string? Address { get; set; }
	}
}
