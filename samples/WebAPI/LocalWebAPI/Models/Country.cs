using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalWebAPI.Models
{
	public class Country
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Population { get; set; }

		public Country(int id, string name, int population)
		{
			Id = id;
			Name = name;
			Population = population;
		}
	}
}
