using Microsoft.AspNetCore.Mvc;
using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.ViewComponents
{
	public class RatingCompo
	{
		public List<Field?> Fields { get; set; }
		public List<Rating?> Ratings { get; set; }
		public List<User>? Users { get; set; }

		public RatingCompo()
		{
			Ratings = new List<Rating>();
			Fields = new List<Field>();
			Users = new List<User>();
		}
	}
}
