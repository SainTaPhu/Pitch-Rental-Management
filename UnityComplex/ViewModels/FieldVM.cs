namespace UnityComplex.ViewModels
{
	public class FieldVM
	{
		public int IdField { get; set; }
		public string? FieldName { get; set; }
		public string? Decription { get; set; }
		public int? MorningPrice { get; set; }
		public int? AfternoonPrice { get; set; }
		public int? IdSport { get; set; }
		public List<string> Images { get; set; }

		public FieldVM()
		{

			Images = new List<string>();
		}
	}
}
