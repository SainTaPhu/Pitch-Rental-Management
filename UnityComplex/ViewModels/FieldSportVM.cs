namespace UnityComplex.ViewModels
{
	public class FieldSportVM
	{
		public int IdField { get; set; }
		public string? FieldName { get; set; }
		public string? Decription { get; set; }
		public int? MorningPrice { get; set; }
		public int? AfternoonPrice { get; set; }
		public int? IdSport { get; set; }
		public List<string> Images { get; set; }
		public string? SportName { get; set; }

		public int? IdUser { get; set; }

		

		





		public FieldSportVM()
		{

			Images = new List<string>();
			
			
		
		}
	}
}
